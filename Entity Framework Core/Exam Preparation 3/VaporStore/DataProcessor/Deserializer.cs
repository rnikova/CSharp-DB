﻿namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Enums;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ImportDtos;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDto = JsonConvert.DeserializeObject<GameDto[]>(jsonString);

            var sb = new StringBuilder();
            var games = new List<Game>();

            foreach (var gameDto in gamesDto)
            {
                if (!IsValid(gameDto) || gameDto.Tags.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };

                var developer = context.Developers.FirstOrDefault(x => x.Name == gameDto.Developer);
                var genre = context.Genres.FirstOrDefault(x => x.Name == gameDto.Genre);

                if (developer == null)
                {
                    developer = new Developer
                    {
                        Name = gameDto.Developer
                    };

                    context.Developers.Add(developer);
                    context.SaveChanges();
                }

                if (genre == null)
                {
                    genre = new Genre
                    {
                        Name = gameDto.Genre.ToString()
                    };

                    context.Genres.Add(genre);
                    context.SaveChanges();
                }

                game.Developer = developer;
                game.Genre = genre;

                foreach (var currentTag in gameDto.Tags)
                {
                    var tag = context.Tags.FirstOrDefault(x => x.Name == currentTag);

                    if (tag == null)
                    {
                        tag = new Tag
                        {
                            Name = currentTag
                        };

                        context.Tags.Add(tag);
                        context.SaveChanges();
                    }

                    game.GameTags.Add(new GameTag
                    {
                        Tag = tag
                    });
                }

                games.Add(game);
                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersDto = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

            var sb = new StringBuilder();
            var users = new List<User>();

            foreach (var userDto in usersDto)
            {
                if (!IsValid(userDto) || !userDto.Cards.Any(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age
                };

                foreach (var card in userDto.Cards)
                {
                    user.Cards.Add(new Card
                    {
                        Number = card.Number,
                        Cvc = card.CVC,
                        Type = Enum.Parse<CardType>(card.Type)
                    });
                }

                users.Add(user);
                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(PurchaseDto[]), new XmlRootAttribute("Purchases"));
            var purchasesDto = (PurchaseDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var purchases = new List<Purchase>();

            foreach (var purchaseDto in purchasesDto)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine("Invalid data");
                    continue;
                }

                var game = context.Games.FirstOrDefault(x => x.Name == purchaseDto.Title);
                var card = context.Cards.FirstOrDefault(x => x.Number == purchaseDto.Card);
                var date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var isValidType = Enum.TryParse<PurchaseType>(purchaseDto.Type, out PurchaseType purchaseType);

                if (game == null || card == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase()
                {
                    Game = game,
                    Type = purchaseType,
                    Card = card,
                    ProductKey = purchaseDto.Key,
                    Date = date
                };

                purchases.Add(purchase);
                sb.AppendLine($"Imported {game.Name} for {purchase.Card.User.Username}");
            }

            context.Purchases.AddRange(purchases);
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