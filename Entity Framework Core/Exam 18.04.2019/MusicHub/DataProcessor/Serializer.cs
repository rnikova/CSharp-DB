namespace MusicHub.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using MusicHub.DataProcessor.ExportDtos;
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Text;
    using System.IO;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsDto = context.Albums
                .Where(x => x.ProducerId == producerId)
                .OrderByDescending(x => x.Price)
                .Select(x => new AlbumDto
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = x.Producer.Name,
                    Songs = x.Songs.Select(y => new SongDto
                    {
                        SongName = y.Name,
                        Price = y.Price.ToString("F2"),
                        Writer = y.Writer.Name
                    })
                    .OrderByDescending(y => y.SongName)
                    .ThenBy(y => y.Writer)
                    .ToList(),
                    AlbumPrice = x.Price.ToString("F2")
                })
                .ToList();

            var json = JsonConvert.SerializeObject(albumsDto, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Where(x => x.Duration.TotalSeconds > duration)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Writer.Name)
                .ThenBy(x => x.SongPerformers.Select(y => $"{y.Performer.FirstName} {y.Performer.LastName}").FirstOrDefault())
                .Select(x => new SongXmlDto
                {
                    SongName = x.Name,
                    Writer = x.Writer.Name,
                    Performer = x.SongPerformers.Select(y => $"{y.Performer.FirstName} {y.Performer.LastName}").FirstOrDefault(),
                    AlbumProducer = x.Album.Producer.Name,
                    Duration = x.Duration.ToString("c")
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(SongXmlDto[]), new XmlRootAttribute("Songs"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), songs, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}