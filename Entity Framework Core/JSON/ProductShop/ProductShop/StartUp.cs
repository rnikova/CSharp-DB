namespace ProductShop
{
    using System;
    using System.IO;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using ProductShop.Data;
    using ProductShop.Models;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            var usersJson = File.ReadAllText(@"C:\Users\Rosmari\Desktop\JSON\ProductShop\ProductShop\Datasets\categories-products.json");

            Console.WriteLine(ImportCategoryProducts(context, usersJson));
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
                .Where(c => c.Name != null);

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);
            
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }
    }
}