﻿namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesDto = JsonConvert.DeserializeObject<MovieDto[]>(jsonString);

            var sb = new StringBuilder();
            var movies = new List<Movie>();

            foreach (var movieDto in moviesDto)
            {
                var isValidGenre = Enum.TryParse<Genre>(movieDto.Genre, out Genre genre);
                var isTitleExist = context.Movies.FirstOrDefault(x => x.Title == movieDto.Title);

                if (!IsValid(movieDto) || !isValidGenre || isTitleExist != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = new Movie
                {
                    Title = movieDto.Title,
                    Genre = genre,
                    Duration = TimeSpan.Parse(movieDto.Duration),
                    Rating = movieDto.Rating,
                    Director = movieDto.Director
                };

                movies.Add(movie);
                sb.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre.ToString(), movie.Rating.ToString("F2")));
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallsDto = JsonConvert.DeserializeObject<HallDto[]>(jsonString);

            var sb = new StringBuilder();
            var halls = new List<Hall>();

            foreach (var hallDto in hallsDto)
            {
                if (!IsValid(hallDto) || hallDto.Seats <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = new Hall
                {
                    Name = hallDto.Name,
                    Is4Dx = hallDto.Is4Dx,
                    Is3D = hallDto.Is3D,
                    Seats = new List<Seat>()
                };

                for (int i = 0; i < hallDto.Seats; i++)
                {
                    hall.Seats.Add(new Seat());
                }

                var type = string.Empty;

                if (hall.Is3D && hall.Is4Dx)
                {
                    type = "4Dx/3D";
                }
                else if (hall.Is3D)
                {
                    type = "3D";
                }
                else if (hall.Is4Dx)
                {
                    type = "4Dx";
                }
                else
                {
                    type = "Normal";
                }

                halls.Add(hall);
                sb.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, type, hall.Seats.Count));
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProjectionDto[]), new XmlRootAttribute("Projections"));
            var projectionsDto = (ProjectionDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var projections = new List<Projection>();

            foreach (var projectionDto in projectionsDto)
            {
                var isValidMovie = context.Movies.Find(projectionDto.MovieId);
                var isValidHall = context.Halls.Find(projectionDto.HallId);

                if (!IsValid(projectionDto) || isValidHall == null || isValidMovie == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var projection = new Projection
                {
                    HallId = projectionDto.HallId,
                    MovieId = projectionDto.MovieId,
                    DateTime = DateTime.ParseExact(projectionDto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                };

                projections.Add(projection);
                sb.AppendLine(string.Format(SuccessfulImportProjection, isValidMovie.Title, projection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            }

            context.Projections.AddRange(projections);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(CustomerDto[]), new XmlRootAttribute("Customers"));
            var customersDto = (CustomerDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var customers = new List<Customer>();

            foreach (var customerDto in customersDto)
            {
                var projections = context.Projections.Select(x => x.Id).ToArray();
                var isProjectionExist = projections.Any(x => customerDto.Tickets.Any(s => s.ProjectionId != x));

                if (!IsValid(customerDto) && customerDto.Tickets.All(IsValid) && isProjectionExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Age = customerDto.Age,
                    Balance = customerDto.Balance,
                };

                foreach (var ticket in customerDto.Tickets)
                {
                    customer.Tickets.Add(new Ticket
                    {
                        ProjectionId = ticket.ProjectionId,
                        Price = ticket.Price
                    });
                }

                sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, customer.FirstName, customer.LastName, customer.Tickets.Count()));

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            var IsValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return IsValid;
        }
    }
}