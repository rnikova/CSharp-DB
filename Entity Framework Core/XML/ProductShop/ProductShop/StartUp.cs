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

            Console.WriteLine(ImportCategoryProducts(context, inputXml));


        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var xml = new XmlSerializer(typeof(ImportCategoryProductsDto[]), new XmlRootAttribute("CategoryProducts"));

            var categoryProductsDto = ((ImportCategoryProductsDto[])xml.Deserialize(new StringReader(inputXml)));

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