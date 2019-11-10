
namespace CarDealer
{
    using System;
    using System.IO;
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using CarDealer.Data;
    using CarDealer.Models;
    using CarDealer.DTO.Import;
    using CarDealer.DTO.Export;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            var inputJson = File.ReadAllText(@"D:\SoftUni\CSharp-DB\Entity Framework Core\JSON\CarDealer\CarDealer\Datasets\sales.json");

            Console.WriteLine(GetCarsFromMakeToyota(context));
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new SupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new CarDto
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new CustomerDto
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYongDriver = c.IsYoungDriver
                })
                .ToList();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return json;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var json = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.AttachRange(json);
            context.SaveChanges();

            return $"Successfully imported {json.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var json = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.AddRange(json);
            context.SaveChanges();

            return $"Successfully imported {json.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var json = JsonConvert.DeserializeObject<ImortCarDto[]>(inputJson);

            foreach (var carDto in json)
            {
                Car car = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                context.Cars.Add(car);

                foreach (var partId in carDto.PartsId)
                {
                    PartCar partCar = new PartCar
                    {
                        CarId = car.Id,
                        PartId = partId
                    };

                    if (car.PartCars.FirstOrDefault(p => p.PartId == partId) == null)
                    {
                        context.PartCars.Add(partCar);
                    }
                }
            }

            context.SaveChanges();

            return $"Successfully imported {json.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var json = JsonConvert.DeserializeObject<Part[]>(inputJson).Where(p => p.SupplierId <= 31);

            context.Parts.AddRange(json);
            context.SaveChanges();

            return $"Successfully imported {json.Count()}.";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var json = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(json);
            context.SaveChanges();

            return $"Successfully imported {json.Count()}.";
        }
    }
}