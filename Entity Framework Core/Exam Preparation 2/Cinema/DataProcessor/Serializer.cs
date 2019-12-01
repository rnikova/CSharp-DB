namespace Cinema.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var moviesDto = context.Movies
                 .Where(m => m.Rating >= rating && m.Projections.Any(t => t.Tickets.Count > 0))
                 .OrderByDescending(m => m.Rating)
                 .ThenByDescending(m => m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)))
                 .Select(m => new ExportMovieDto
                 {
                     MovieName = m.Title,
                     Rating = m.Rating.ToString("F2"),
                     TotalIncomes = m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("F2"),
                     Customers = m.Projections.SelectMany(t => t.Tickets).Select(c => new ExportCustomerDto
                     {
                         FirstName = c.Customer.FirstName,
                         LastName = c.Customer.LastName,
                         Balance = c.Customer.Balance.ToString("F2")
                     })
                     .OrderByDescending(b => b.Balance)
                     .ThenBy(f => f.FirstName)
                     .ThenBy(l => l.LastName)
                     .ToList()
                 })
                 .Take(10)
                 .ToList();

            var json = JsonConvert.SerializeObject(moviesDto, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customersDto = context.Customers
                .Where(c => c.Age >= age)
                .OrderByDescending(c => c.Tickets.Sum(t => t.Price))
                .Select(c => new ExportCustomerXmlDto
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    SpentMoney = c.Tickets.Sum(t => t.Price).ToString("F2"),
                    SpentTime = TimeSpan.FromSeconds(c.Tickets.Sum( t => t.Projection.Movie.Duration.TotalSeconds)).ToString(@"hh\:mm\:ss")
                })
                .Take(10)
                .ToArray();


            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(ExportCustomerXmlDto[]), new XmlRootAttribute("Customers"));

            serializer.Serialize(new StringWriter(sb), customersDto, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}