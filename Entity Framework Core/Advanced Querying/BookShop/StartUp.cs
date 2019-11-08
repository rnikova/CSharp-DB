namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // DbInitializer.ResetDatabase(db);

                Console.WriteLine(GetAuthorNamesEndingIn(db, "e"));
            }
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context
                .Books
                .Where(b => b.Title.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context
                .Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToList();

            var result = new StringBuilder();

            foreach (var author in authors)
            {
                result.AppendLine($"{author.FullName}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateTime = DateTime.ParseExact(date, "dd-MM-yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            var books = context
                .Books
                .Select(b => new
                {
                    b.ReleaseDate,
                    b.Title,
                    b.Price,
                    b.EditionType
                })
                .Where(b => b.ReleaseDate < dateTime)
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            var result = new StringBuilder();

            foreach (var book in books)
            {
                result.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToLower()).ToArray();

            var books = context
                .Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .Select(t => t.Title)
                .OrderBy(x => x)
                .ToList();

            var result = string.Join(Environment.NewLine, books);

            return result;
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context
                .Books
                .Select(b => new
                {
                    b.ReleaseDate,
                    b.BookId,
                    b.Title
                })
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .ToList();

            var result = new StringBuilder();

            foreach (var book in books)
            {
                result.AppendLine(book.Title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context
                .Books
                .Select(b => new
                {
                    b.Price,
                    b.Title
                })
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .ToList();

            var result = new StringBuilder();

            foreach (var book in books)
            {
                result.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context
                .Books
                .Where(e => e.EditionType == EditionType.Gold && e.Copies < 5000)
                .Select(t => new 
                { 
                    Title = t.Title, 
                    Id = t.BookId
                })
                .OrderBy(x => x.Id)
                .ToList();

            var result = new StringBuilder();

            foreach (var book in books)
            {
                result.AppendLine(book.Title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context
                .Books
                .Where(a => a.AgeRestriction == ageRestriction)
                .Select(t => t.Title)
                .OrderBy(x => x)
                .ToList();

            var result = string.Join(Environment.NewLine, books);

            return result;
        }
    }
}
