namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(BookDto[]), new XmlRootAttribute("Books"));
            var booksDto = (BookDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var books = new List<Book>();

            foreach (var bookDto in booksDto)
            {
                var isValidGenre = Enum.TryParse<Genre>(bookDto.Genre.ToString(), out Genre genre);

                if (!IsValid(bookDto) || bookDto.Genre < 1 || bookDto.Genre > 3)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var book = new Book
                {
                    Name = bookDto.Name,
                    Genre = genre,
                    Price = bookDto.Price,
                    Pages = bookDto.Pages,
                    PublishedOn = DateTime.ParseExact(bookDto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                };

                books.Add(book);
                sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price.ToString("F2")));
            }

            context.Books.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsDto = JsonConvert.DeserializeObject<AuthorDto[]>(jsonString);

            var sb = new StringBuilder();
            var authors = new List<Author>();

            foreach (var authorDto in authorsDto)
            {
                var isExistEmail = context.Authors.Any(x => x.Email == authorDto.Email);

                if (!IsValid(authorDto) || isExistEmail)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var author = new Author
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Phone = authorDto.Phone,
                    Email = authorDto.Email,
                    //AuthorsBooks = new List<AuthorBook>()
                };

                if (authorDto.Books.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                context.Authors.Add(author);

                foreach (var bookDto in authorDto.Books)
                {
                    var isExistBook = context.Books.Find(bookDto.Id);

                    if (isExistBook == null || !IsValid(bookDto))
                    {
                        continue;
                    }

                    var book = new AuthorBook
                    {
                        Author = author,
                        BookId = (int)bookDto.Id
                    };

                    author.AuthorsBooks.Add(book);
                    context.AuthorsBooks.Add(book);
                    context.SaveChanges();
                }


                var authorFullName = $"{author.FirstName} {author.LastName}";

                authors.Add(author);
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, authorFullName, author.AuthorsBooks.Count));
            }

            //context.Authors.AddRange(authors);
            //context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}