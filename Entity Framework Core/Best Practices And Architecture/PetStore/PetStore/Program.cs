using PetStore.Data;
using PetStore.Services.Implementations;
using System;

namespace PetStore
{
    class Program
    {
        static void Main(string[] args)
        {
            using var data = new PetStoreDbContext();

            var brandService = new BrandService(data);

            var userService = new UserService(data);
            var foodService = new FoodService(data, userService);

            var toyService = new ToyService(data, userService);
        }
    }
}
