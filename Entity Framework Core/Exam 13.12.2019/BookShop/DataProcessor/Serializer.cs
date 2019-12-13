namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new ExportAuthorDto
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    Books = a.AuthorsBooks
                    .OrderByDescending(b => b.Book.Price)
                    .Select(b => new ExportBookDto
                    {
                        BookName = b.Book.Name,
                        BookPrice = b.Book.Price.Value.ToString("F2")
                    })
                    .ToList()
                })
                .ToList()
                .OrderByDescending(a => a.Books.Count)
                .ThenBy(a => a.AuthorName)
                .ToList();

            var json = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return json;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var books = context.Books
                .OrderByDescending(x => x.Pages)
                .ThenByDescending(x => x.PublishedOn)
                .Where(x => x.PublishedOn < date && x.Genre.ToString() == "Science")
                .Select(b => new ExportBookXmlDto
                {
                    Pages = (int)b.Pages,
                    Name = b.Name,
                    Date = b.PublishedOn.ToString("d", CultureInfo.InvariantCulture)
                })
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportBookXmlDto[]), new XmlRootAttribute("Books"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), books, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}