namespace MiniORM
{
    using System.Collections.Generic;
    using System.Linq;

    internal class ChangeTracker<T>
        where T : class, new()
    {
        private readonly List<T> allEntities;

        private readonly List<T> added;

        private readonly List<T> removed;

        public ChangeTracker(IEnumerable<T> entities)
        {
            this.added = new List<T>();
            this.removed = new List<T>();

            this.allEntities = CloneEntities(entities);
        }

        private static List<T> CloneEntities(IEnumerable<T> entities)
        {
            List<T> clonedEntities = new List<T>();

            var propertiesToClone = typeof(T).GetProperties().Where(x => DBContext)
        }
    }
}