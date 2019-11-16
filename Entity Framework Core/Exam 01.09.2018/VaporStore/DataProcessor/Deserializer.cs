namespace VaporStore.DataProcessor
{
    using System;
    using Newtonsoft.Json;
    using Data;
    using VaporStore.DataProcessor.ImportDtos;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using VaporStore.Data.Models;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDto = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var gameDto in gamesDto)
            {
                if (!IsValid(gamesDto) || gameDto.Tags.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var developer = GetDeveloper(context, gameDto.Developer);
                var genre = GetGenre(context, gameDto.Genre);

                foreach (var tag in gameDto.Tags)
                {
                    var taf = GetTag(context, tag);
                }
            }

            return null;
        }

        private static Tag GetTag(VaporStoreDbContext context, string tagString)
        {
            var tag = context.Tags.FirstOrDefault(x => x.Name == tagString);

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagString
                };

                context.Tags.Add(tag);
                context.SaveChanges();
            }

            return tag;
        }

        private static Genre GetGenre(VaporStoreDbContext context, string genreString)
        {
            var genre = context.Genres.FirstOrDefault(x => x.Name == genreString);

            if (genre == null)
            {
                genre = new Genre
                {
                    Name = genreString
                };

                context.Genres.Add(genre);
                context.SaveChanges();
            }

            return genre;
        }

        private static Developer GetDeveloper(VaporStoreDbContext context, string stringDeveloper)
        {
            var developer = context.Developers.FirstOrDefault(x => x.Name == stringDeveloper);

            if (developer == null)
            {
                developer = new Developer
                {
                    Name = stringDeveloper
                };

                context.Developers.Add(developer);
                context.SaveChanges();
            }

            return developer;
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            throw new NotImplementedException();
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