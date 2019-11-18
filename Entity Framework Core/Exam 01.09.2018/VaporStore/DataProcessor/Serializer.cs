namespace VaporStore.DataProcessor
{
	using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Enums;
    using VaporStore.DataProcessor.ExportDtos;
    using Formatting = Newtonsoft.Json.Formatting;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
            var games = context.Genres
                .Where(x => genreNames.Contains(x.Name))
                .Select(x => new ExportGenreDto
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games
                        .Where(p => p.Purchases.Any())
                        .Select(g => new ExportGameDto
                        {
                            Id = g.Id,
                            Title = g.Name,
                            Developer = g.Developer.Name,
                            Tags = string.Join(", ", g.GameTags.Select(t => t.Tag.Name)),
                            Players = g.Purchases.Count
                        })
                        .OrderByDescending(g => g.Players)
                        .ThenBy(g => g.Id)
                        .ToList(),
                    TotalPlayers = x.Games.Sum(p => p.Purchases.Count)
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id)
                .ToList();

            var json = JsonConvert.SerializeObject(games, Formatting.Indented);

            return json;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
            var storeTypeValue = Enum.Parse<PurchaseType>(storeType);

            var purchases = context.Users
                .Select(x => new ExportUserDto
                {
                    Username = x.Username,
                    Purchases = x.Cards
                        .SelectMany(c => c.Purchases)
                        .Where(p => p.Type == storeTypeValue)
                        .Select(p => new ExportPurchaseDto
                        {
                            Card = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportPurchaseGameDto
                            {
                                Title = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price,
                            }
                        })
                        .OrderBy(p => p.Date)
                        .ToArray(),
                    TotalSpent = x.Cards
                        .SelectMany(c => c.Purchases)
                        .Where(p => p.Type == storeTypeValue)
                        .Sum(p => p.Game.Price)
                })
                .Where(u => u.Purchases.Any())
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportUserDto[]), new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), purchases, namespaces);

            return sb.ToString();
        }
	}
}