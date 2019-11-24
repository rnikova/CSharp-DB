namespace Cinema.DataProcessor
{
    using System;
    using System.Linq;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    using Data;
    using Cinema.DataProcessor.ExportDto;
    using System.IO;
    using System.Text;
    using System.Xml;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var customersDto = context.Movies
                .Where(x => x.Projections.Any(m => m.Tickets.Count() > 0) && x.Rating >= rating)
                .OrderByDescending(x => x.Rating)
                .ThenByDescending(x => x.Projections.Sum(i => i.Tickets.Sum(t => t.Price)))
                .Select(x => new MovieDto
                {
                    MovieName = x.Title,
                    Rating = x.Rating.ToString("F2"),
                    TotalIncomes = x.Projections.Sum(i => i.Tickets.Sum(t => t.Price)).ToString("F2"),
                    Customers = x.Projections
                   .SelectMany(t => t.Tickets)
                        .Select(c => new CustomerDto
                        {
                            FirstName = c.Customer.FirstName,
                            LastName = c.Customer.LastName,
                            Balance = c.Customer.Balance.ToString("F2")
                        })
                        .OrderByDescending(b => b.Balance)
                        .ThenBy(f => f.FirstName)
                        .ThenBy(l => l.LastName)
                        .ToArray()
                })
                .Take(10)
                .ToList();

            var json = JsonConvert.SerializeObject(customersDto, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customersDto = context.Customers
                .Where(x => x.Age >= age)
                .OrderByDescending(x => x.Tickets.Sum(t => t.Price))
                .Select(x => new CustomerXmlDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SpentMoney = x.Tickets.Sum(t => t.Price).ToString("F2"),
                    SpentTime = TimeSpan.FromSeconds(x.Tickets.Sum(t => t.Projection.Movie.Duration.TotalSeconds)).ToString(@"hh\:mm\:ss")
                })
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CustomerXmlDto[]), new XmlRootAttribute("Customers"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), customersDto, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}