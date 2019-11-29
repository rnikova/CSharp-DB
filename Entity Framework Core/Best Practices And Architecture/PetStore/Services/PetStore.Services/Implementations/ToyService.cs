using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.Toy;
using System;
using System.Linq;

namespace PetStore.Services.Implementations
{
    public class ToyService : IToyService
    {

        private readonly PetStoreDbContext data;
        private readonly UserService userService;

        public ToyService(PetStoreDbContext data, UserService userService) 
        { 
            this.data = data;
            this.userService = userService;
        }

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

        public bool IsExist(int toyId)
        {
            return this.data.Toys.Any(t => t.Id == toyId);
        }

        public void SellToUser(int toyId, int userId)
        {
            if (!this.IsExist(toyId))
            {
                throw new ArgumentException("There is no such toy with given Id in the database!");
            }

            if (!this.userService.IsExist(userId))
            {
                throw new ArgumentException("There is no such user with given Id in the database!");
            }

            var order = new Order()
            {
                PurchaseDate = DateTime.Now,
                Status = OrderStatus.Done,
                UserId = userId
            };

            var toyOrder = new ToyOrder()
            {
                ToyId = toyId,
                Order = order
            };

            this.data.Orders.Add(order);
            this.data.ToyOrders.Add(toyOrder);
            this.data.SaveChanges();
        }
    }
}
