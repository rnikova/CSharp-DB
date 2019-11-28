using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.Toy;
using System;

namespace PetStore.Services.Implementations
{
    public class ToyService : IToyService
    {

        private readonly PetStoreDbContext data;

        public ToyService(PetStoreDbContext data) => this.data = data;

        public void ByToyByDistributor(string name, string description, decimal distributorPrice, decimal profit, int brandId, int categoryId)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace");
            }

            if (profit < 0)
            {
                throw new ArgumentException("Profit must be figher than zero");
            }

            var toy = new Toy()
            {
                Name = name,
                Description = description,
                DistributorPrice = distributorPrice,
                Price = distributorPrice + (distributorPrice * profit),
                BrandId = brandId,
                CategoryId = categoryId
            };

            this.data.Toys.Add(toy);
            this.data.SaveChanges();
        }

        public void ByToyByDistributor(AddingToyServiceModel model)
        {
            if (String.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Name cannot be null or whitespace");
            }

            if (model.Profit < 0)
            {
                throw new ArgumentException("Profit must be figher than zero");
            }

            var toy = new Toy()
            {
                Name = model.Name,
                Description = model.Description,
                DistributorPrice = model.Price,
                Price = model.Price + (model.Price * model.Profit),
                BrandId = model.BrandId,
                CategoryId = model.CategoryId
            };

        }
    }
}
