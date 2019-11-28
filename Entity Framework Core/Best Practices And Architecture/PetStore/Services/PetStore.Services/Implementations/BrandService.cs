using System;
using System.Collections.Generic;
using System.Linq;
using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.Brand;
using PetStore.Services.Models.Toy;

namespace PetStore.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly PetStoreDbContext data;

        public BrandService(PetStoreDbContext data) => this.data = data;

        public int Create(string name)
        {
            if (name.Length > DataValidation.NameMaxLegth)
            {
                throw new InvalidOperationException($"Brand name can not be more than {DataValidation.NameMaxLegth} characters.");
            }

            if (this.data.Brands.Any(b => b.Name == name))
            {
                throw new InvalidOperationException($"Brand name  {name} already exists.");
            }

            var brand = new Brand
            {
                Name = name
            };

            this.data.Brands.Add(brand);

            this.data.SaveChanges();

            return brand.Id;
        }

        public BrandWithToysServiceModel FindByIdWithToys(int id)
        => this.data.Brands
            .Where(b => b.Id == id)
            .Select(b => new BrandWithToysServiceModel
            {
                Name = b.Name,
                Toys = b.Toys.Select(x => new ToyListingServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    TotalOrders = x.Orders.Count
                })
            })
            .FirstOrDefault();

        public IEnumerable<BrandListingServiceModel> SearchByName(string name)
            => this.data.Brands
                .Where(b => b.Name.ToLower().Contains(name.ToLower()))
                .Select(b => new BrandListingServiceModel
                {
                    Id = b.Id,
                    Name = b.Name
                })
            .ToList();
    }
}
