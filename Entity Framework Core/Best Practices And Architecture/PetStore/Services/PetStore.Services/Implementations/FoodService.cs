using System;
using System.Linq;
using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.Food;

namespace PetStore.Services.Implementations
{
    public class FoodService : IFoodService
    {
        private readonly PetStoreDbContext data;

        public FoodService(PetStoreDbContext data) => this.data = data;

        public void BuyFromDistributor(string name, double weight, decimal price, decimal profit, DateTime expirationDate, int brandId, int categoryId)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace");
            }

            if (profit < 0)
            {
                throw new ArgumentException("Profit must be figher than zero");
            }
            
            var food = new Food()
            {
                Name = name,
                Weigth = weight,
                Price = price + (price * profit),
                ExpirationDate = expirationDate,
                BrandId = brandId,
                CategoryId = categoryId
            };

            this.data.Food.Add(food);
            this.data.SaveChanges();
        }

        public void BuyFromDistributor(AddingFoodServiceModel model)
        {
            if (String.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Name cannot be null or whitespace");
            }

            if (model.Profit < 0)
            {
                throw new ArgumentException("Profit must be figher than zero");
            }

            var food = new Food()
            {
                Name = model.Name,
                Weigth = model.Weight,
                DistributorPrice = model.Price,
                Price = model.Price + (model.Price * model.Profit),
                ExpirationDate = model.ExpirationDate,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId
            };

            this.data.Food.Add(food);
            this.data.SaveChanges();
        }
    }
}
