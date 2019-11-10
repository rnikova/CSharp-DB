namespace ProductShop
{
    using System;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using ProductShop.Dto;
    using ProductShop.Data;
    using ProductShop.Models;
    using Newtonsoft.Json.Serialization;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            //var inputJson = File.ReadAllText(@"D:\SoftUni\CSharp-DB\Entity Framework Core\JSON\ProductShop\ProductShop\Datasets\categories-products.json");

            Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(p => p.ProductsSold.Count(ps => ps.Buyer != null))
                .Select(u => new UsersWithProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsToUserDto
                    {
                        Count = u.ProductsSold
                            .Count(p => p.Buyer != null),
                        Products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(ps => new SoldProductsDto
                            {
                                Name = ps.Name,
                                Price = ps.Price
                            })
                            .ToList()
                    }
                })
                .ToList();

            var result = new 
            {
                UsersCount = users.Count(),
                Users = users
            };

            var contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(result,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = contractResolver,
                    NullValueHandling = NullValueHandling.Ignore
                });

            return json;

        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new CategoriesByProductsCountDto
                {
                    CategoryName = c.Name,
                    ProductsCount = c.CategoryProducts.Count(),
                    AveragePrice = $"{c.CategoryProducts.Average(p => p.Product.Price):f2}",
                    TotalRevenue = $"{c.CategoryProducts.Sum(p => p.Product.Price):f2}"
                })
                .OrderByDescending(c => c.ProductsCount)
                .ToList();

            var json = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .Select(u => new UserSoldProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(ps => new SoldProductsDto
                        {
                            Name = ps.Name,
                            Price = ps.Price,
                            BuyerFirstName = ps.Buyer.FirstName,
                            BuyerLastName = ps.Buyer.LastName
                        })
                        .ToList()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .OrderBy(p => p.Price)
                .ToList();

            var jsonProducts = JsonConvert.SerializeObject(products, Formatting.Indented);

            return jsonProducts;
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