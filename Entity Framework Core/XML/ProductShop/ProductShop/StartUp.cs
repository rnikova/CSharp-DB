namespace ProductShop
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    using AutoMapper;

    using ProductShop.Data;
    using ProductShop.Models;
    using ProductShop.Dtos.Import;
    using System.Linq;
    using ProductShop.Dtos.Export;
    using System.Text;
    using System.Xml;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            Mapper.Initialize(x =>
            {
                x.AddProfile<ProductShopProfile>();
            });

            var inputXml = File.ReadAllText(@"..\..\..\Datasets\categories-products.xml");

            Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(p => p.ProductsSold.Count())
                .Select(u => new UsersWithSoldProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsCountDto
                    {
                        Count = u.ProductsSold.Count(),
                        Products = u.ProductsSold
                        .Select(p => new SoldProductsDto
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }
                })
                .Take(10)
                .ToArray();

            var result = new UsersAndProductsDto
            {
                Count = context.Users.Count(p => p.ProductsSold.Any()),
                Users = users
            };

            var xmlSerializer = new XmlSerializer(typeof(UsersAndProductsDto), new XmlRootAttribute("Users"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(sb), result, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(x => new ExportCategoriesByProductCountDto
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Sum(p => p.Product.Price) / x.CategoryProducts.Count(),
                    TotalRevenue = x.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportCategoriesByProductCountDto[]), new XmlRootAttribute("Categories"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(y => y.Buyer != null))
                .Select(x => new ExportUserWithSoldProductsDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Products = x.ProductsSold.Select(p => new ExportSoldProductDto
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToArray()
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportUserWithSoldProductsDto[]), new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new ExportProductInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .Take(10)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportProductInRangeDto[]), new XmlRootAttribute("Products"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var xml = new XmlSerializer(typeof(ImportCategoryProductsDto[]), new XmlRootAttribute("CategoryProducts"));

            var categoryProductsDto = (ImportCategoryProductsDto[])xml.Deserialize(new StringReader(inputXml));

            var categoryProducts = new List<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductsDto)
            {
                var product = context.Products.Find(categoryProductDto.ProductId);
                var category = context.Categories.Find(categoryProductDto.CategoryId);

                if (product != null && category != null)
                {
                    var currentCategory = Mapper.Map<CategoryProduct>(categoryProductDto);
                    categoryProducts.Add(currentCategory);
                }
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var xml = new XmlSerializer(typeof(ImportCategoryDto[]), new XmlRootAttribute("Categories"));

            var categoriesDto = (ImportCategoryDto[])xml.Deserialize(new StringReader(inputXml));

            var categories = new List<Category>();

            foreach (var categoryDto in categoriesDto)
            {
                var category = Mapper.Map<Category>(categoryDto);
                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var xml = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));

            var productsDto = (ImportProductDto[])xml.Deserialize(new StringReader(inputXml));

            var products = new List<Product>();

            foreach (var productDto in productsDto)
            {
                var product = Mapper.Map<Product>(productDto);
                products.Add(product);
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var xml = new XmlSerializer(typeof(ImportUserDto[]), new XmlRootAttribute("Users"));

            var usersDto = (ImportUserDto[])xml.Deserialize(new StreamReader(inputXml));

            var users = new List<User>();

            foreach (var userDto in usersDto)
            {
                var user = Mapper.Map<User>(userDto);
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
    }
}