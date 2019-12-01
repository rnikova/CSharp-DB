using System;
using System.Collections.Generic;
using System.Linq;
using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.Pet;

namespace PetStore.Services.Implementations
{
    public class PetService : IPetService
    {
        private readonly PetStoreDbContext data;
        private readonly IBreedService breedService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;

        public PetService(PetStoreDbContext data, IBreedService breedService, ICategoryService categoryService)
        {
            this.data = data;
            this.breedService = breedService;
            this.categoryService = categoryService;
        }

        public void BuyPet(Gender gender, DateTime dateOfBirth, decimal price, string description, int breedId, int categoryId)
        {
            if (price < 0)
            {
                throw new ArgumentException("Price of the pet cannot be less than zero!");
            }

            if (!this.breedService.IsExist(breedId))
            {
                throw new ArgumentException("There is no such breed with given Id in the database!");
            }

            if (!this.categoryService.IsExist(categoryId))
            {
                throw new ArgumentException("There is no such category with given Id in the database!");
            }

            var pet = new Pet()
            {
                Gender = gender,
                DateOfBirth = dateOfBirth,
                Price = price,
                Description = description,
                BreedId = breedId,
                CategoryId = categoryId
            };

            this.data.Pets.Add(pet);
            this.data.SaveChanges();
        }

        public void SellPet(int petId, int userId)
        {
            if (!this.userService.IsExist(userId))
            {
                throw new ArgumentException("There is no such user with given Id in the database!");
            }

            if (!this.IsExist(petId))
            {
                throw new ArgumentException("There is no such pet with given Id in the database!");
            }

            var order = new Order()
            {
                PurchaseDate = DateTime.Now,
                Status = OrderStatus.Done,
                UserId = userId
            };

            var pet = this.data.Pets.First(p => p.Id == petId);

            this.data.Orders.Add(order);
            pet.Order = order;

            this.data.SaveChanges();
        }

        public bool IsExist(int petId)
        {
            return this.data.Pets.Any(p => p.Id == petId);
        }

        public IEnumerable<PetListingServiceModel> All()
        => this.data.Pets
            .Select(p => new PetListingServiceModel
            {
                Id = p.Id,
                Category = p.Category.Name,
                Price = p.Price,
                Breed = p.Breed.Name
            })
            .ToList();
    }
}
