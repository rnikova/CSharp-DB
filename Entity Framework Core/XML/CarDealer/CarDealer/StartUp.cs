namespace CarDealer
{
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.Dtos.Import;
    using CarDealer.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            Mapper.Initialize(x =>
            {
                x.AddProfile<CarDealerProfile>();
            });

            var inputXml = File.ReadAllText(@"..\..\..\Datasets\sales.xml");

            Console.WriteLine(ImportSales(context, inputXml));
        }

        //public static string GetCarsWithDistance(CarDealerContext context)
        //{

        //}

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(SaleDto[]), new XmlRootAttribute("Sales"));

            var salesDto = (SaleDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var sales = new List<Sale>();

            foreach (var saleDto in salesDto)
            {
                var sale = Mapper.Map<Sale>(saleDto);

                if (context.Cars.Find(saleDto.CarId) != null)
                {
                    sales.Add(sale);
                }
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(CustomerDto[]), new XmlRootAttribute("Customers"));

            var customersDto = (CustomerDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var customers = new List<Customer>();

            foreach (var customerDto in customersDto)
            {
                var customer = Mapper.Map<Customer>(customerDto);
                customers.Add(customer);
            }

            context.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(CarDto[]), new XmlRootAttribute("Cars"));

            var carsDto = (CarDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var cars = new List<Car>();

            foreach (var carDto in carsDto)
            {
                var car = Mapper.Map<Car>(carDto);

                context.Cars.Add(car);

                foreach (var part in carDto.Parts.PartsId)
                {
                    if (car.PartCars.FirstOrDefault(p => p.PartId == part.PartId) == null &&
                        context.Parts.Find(part.PartId) != null)
                    {
                        var partCar = new PartCar
                        {
                            CarId = car.Id,
                            PartId = part.PartId
                        };

                        car.PartCars.Add(partCar);
                    }
                }

                cars.Add(car);
            }

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(PartDto[]), new XmlRootAttribute("Parts"));

            var partsDto = (PartDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var parts = new List<Part>();

            foreach (var partDto in partsDto)
            {
                var supplier = context.Suppliers.Find(partDto.SupplierId);

                if (supplier != null)
                {
                    var part = Mapper.Map<Part>(partDto);
                    parts.Add(part);
                }
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(SupplierDto[]), new XmlRootAttribute("Suppliers"));

            var suppliersDto = (SupplierDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var suppliers = new List<Supplier>();

            foreach (var supplierDto in suppliersDto)
            {
                var supplier = Mapper.Map<Supplier>(supplierDto);
                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }
    }
}