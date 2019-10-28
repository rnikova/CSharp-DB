namespace MiniORM
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class DbContext
    {
        private readonly DatabaseConnection connection;

        private readonly Dictionary<Type, PropertyInfo> DbSetProperties;

        protected DbContext(string connectionString)
        {
            this.connection = new DatabaseConnection(connectionString);

            this.DbSetProperties = this.DiscoverDbSet();

            using (new ConnectionManager(this.connection))
            {
                this.InitializeDbSets();
            }

            this.MapAllRelations();
        }

        internal static readonly Type[] AllowedSqlTypes =
        {
            typeof(string),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(decimal),
            typeof(bool),
            typeof(DateTime),
        };

        public void SaveChanges()
        {
            var dbSets = this.DbSetProperties
                .Select(x => x.Value.GetValue(this))
                .ToArray();

            foreach (IEnumerable<object> dbSet in dbSets)
            {
                var invalidEntities = dbSet.Where(x => !IsObjectValid(x)).ToArray();

                if (invalidEntities.Any())
                {
                    throw new InvalidOperationException($"{invalidEntities.Length} Invalid Entities found in {dbSet.GetType().Name}!");
                }
            }

            using (new ConnectionManager(this.connection))
            {
                using (var transaction = this.connection.StartTransaction())
                {
                    foreach (var dbSet in dbSets)
                    {
                        var dbSetType = dbSet.GetType().GetGenericArguments().First();

                        var persistMethod = typeof(DbContext)
                            .GetMethod("Persist", BindingFlags.Instance | BindingFlags.NonPublic)
                            .MakeGenericMethod(dbSetType);

                        try
                        {
                            persistMethod.Invoke(this, new object[] { dbSet});
                        }
                        catch (TargetInvocationException tie)
                        {

                            throw tie.InnerException;
                        }
                        catch (InvalidOperationException)
                        {
                            transaction.Rollback();
                            throw;
                        }
                        catch (SqlException)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        private void Persist<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            var tableName = this.GetTableName(typeof(TEntity));

            var columns = this.connection.FetchColumnNames(tableName).ToArray();

            if (dbSet.ChangeTracker.Added.Any())
            {
                this.connection.InsertEntities(dbSet.ChangeTracker.Added, tableName, columns);
            }

            var modifiedEntities = dbSet.ChangeTracker.GetModifiedEntities(dbSet).ToArray();

            if (modifiedEntities.Any())
            {
                this.connection.UpdateEntities(modifiedEntities, tableName, columns);
            }

            if (dbSet.ChangeTracker.Removed.Any())
            {
                this.connection.DeleteEntities(dbSet.ChangeTracker.Removed, tableName, columns);
            }
        }

        private void InitializeDbSets()
        {
            foreach (var dbSet in this.DbSetProperties)
            {
                var dbSetType = dbSet.Key;
                var dbSetProperty = dbSet.Value;

                var populateDbSetGeneric = typeof(DbContext)
                    .GetMethod("PopulateDbSet", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType);

                populateDbSetGeneric.Invoke(this, new object[] { dbSetProperty});
            }
        }

        private void PopulateDbSet<TEntity>(PropertyInfo dbSet)
            where TEntity : class, new()
        {
            var entities = LoadTableEntities<TEntity>();

            var dbSetInstance = new DbSet<TEntity>(entities);

            ReflectionHelper.ReplaceBackingField(this, dbSet.Name, dbSetInstance);
        }

        private void MapAllRelations()
        {
            foreach (var dbSetProperty in this.DbSetProperties)
            {
                var dbSetType = dbSetProperty.Key;

                var mapRelationGeneric = typeof(DbContext)
                    .GetMethod("MapRelation", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType);

                var dbSet = dbSetProperty.Value.GetValue(this);

                mapRelationGeneric.Invoke(this, new[] { dbSet});
            }
        }

        private void MapRelation<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            var entityType = typeof(TEntity);

            MapNavigationProperties(dbSet);

            var collections = entityType
                .GetProperties()
                .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                .ToArray();

            foreach (var collection in collections)
            {
                var collectionType = collection.PropertyType.GenericTypeArguments.First();

                var mapCollectionMethod = typeof(DbContext)
                    .GetMethod("MapCollection", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(entityType, collectionType);

                mapCollectionMethod.Invoke(this, new object[] { dbSet, collection });
            }
        }

        private void MapCollection<TDbSet, TCollection>(DbSet<TDbSet> dbSet, PropertyInfo collectionProperty)
            where TDbSet : class, new()
            where TCollection : class, new()
        {
            var entityType = typeof(TDbSet);
            var collectionType = typeof(TCollection);

            var primaryKeys = collectionType
                .GetProperties()
                .Where(x => x.HasAttribute<KeyAttribute>())
                .ToArray();

            var primaryKey = primaryKeys.First();

            var foreignKey = entityType
                .GetProperties()
                .First(x => x.HasAttribute<KeyAttribute>());

            var isManyToMany = primaryKeys.Length >= 2;

            if (isManyToMany)
            {
                primaryKey = collectionType
                    .GetProperties()
                    .First(x => collectionType
                            .GetProperty(x.GetCustomAttribute<ForeignKeyAttribute>().Name)
                    .PropertyType == entityType);
            }

            var navigationDbSet = (DbSet<TCollection>)this.DbSetProperties[collectionType].GetValue(this);

            foreach (var entity in dbSet)
            {
                var primaryKeyValue = foreignKey.GetValue(entity);
                var navigationEntities = navigationDbSet
                    .Where(navigationEntity => primaryKey.GetValue(navigationEntity).Equals(primaryKeyValue))
                    .ToArray();

                ReflectionHelper.ReplaceBackingField(entity, collectionProperty.Name, navigationEntities);

            }
        }

        private void MapNavigationProperties<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            var entityType = typeof(TEntity);

            var foreignKeys = entityType
                .GetProperties()
                .Where(x => x.HasAttribute<ForeignKeyAttribute>())
                .ToArray();

            foreach (var foreingKey in foreignKeys)
            {
                var navigationPropertyName = foreingKey.GetCustomAttribute<ForeignKeyAttribute>().Name;
                var navigationProperty = entityType.GetProperty(navigationPropertyName);

                var navigationDbSet = this.DbSetProperties[navigationProperty.PropertyType].GetValue(this);

                var navigationPrimaryKey = navigationProperty
                    .PropertyType
                    .GetProperties()
                    .First(x => x.HasAttribute<KeyAttribute>());

                foreach (var entity in dbSet)
                {
                    var foreignKeyValue = foreingKey.GetValue(entity);
                    var navigationPropertyValue = ((IEnumerable<object>)navigationDbSet)
                        .First(currentNavigationProperty => navigationPrimaryKey.GetValue(currentNavigationProperty).Equals(foreignKeyValue));

                    navigationProperty.SetValue(entity, navigationPropertyValue);
                }
            }
        }

        private static bool IsObjectValid(object e)
        {
            var validationContext = new ValidationContext(e);
            var validationError = new List<ValidationResult>();

            var validationResult = Validator.TryValidateObject(e, validationContext, validationError, validateAllProperties: true);

            return validationResult;
        }

        private IEnumerable<TEntity> LoadTableEntities<TEntity>()
            where TEntity : class
        {
            var table = typeof(TEntity);
            var columns = GetEntityColumnNames(table);
            var tableName = GetTableName(table);
            var fetchedRows = this.connection.FetchResultSet<TEntity>(tableName, columns).ToArray();

            return fetchedRows;
        }

        private string GetTableName(Type tableType)
        {
            var tableName = ((TableAttribute)Attribute.GetCustomAttribute(tableType, typeof(TableAttribute)))?.Name;

            if (tableName == null)
            {
                tableName = this.DbSetProperties[tableType].Name;
            }

            return tableName;
        }

        private Dictionary<Type, PropertyInfo> DiscoverDbSet()
        {
            var dbSets = this
                .GetType()
                .GetProperties()
                .Where(x => x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToDictionary(k => k.PropertyType.GetGenericArguments().First(), v => v);

            return dbSets;
        }

        private string[] GetEntityColumnNames(Type table)
        {
            var tableName = this.GetTableName(table);
            var dbColumns = this.connection.FetchColumnNames(tableName);

            var columns = table
                .GetProperties()
                .Where(x => dbColumns.Contains(x.Name)
                        && !x.HasAttribute<NotMappedAttribute>()
                        && AllowedSqlTypes.Contains(x.PropertyType))
                .Select(x => x.Name)
                .ToArray();

            return columns;

        }
    }
}