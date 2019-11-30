namespace MusicHub.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using MusicHub.DataProcessor.ExportDtos;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsDto = context.Albums
                .Where(p => p.ProducerId == producerId)
                .OrderByDescending(a => a.Price)
                .Select(a => new AlbumDto
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new SongDto
                    {
                        SongName = s.Name,
                        Price = s.Price.ToString("F2"),
                        Writer = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(w => w.Writer)
                    .ToList(),
                    AlbumPrice = a.Price.ToString("F2")
                })
                .ToList();

            var json = JsonConvert.SerializeObject(albumsDto, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songsDto = context.Songs
                .Where(s => s.Duration.TotalSeconds > duration)
                .OrderBy(s => s.Name)
                .ThenBy(w => w.Writer.Name)
                .ThenBy(s => s.SongPerformers.Select(p => $"{p.Performer.FirstName} {p.Performer.LastName}").FirstOrDefault())
                .Select(s => new SongXmlDto
                {
                    SongName = s.Name,
                    Writer = s.Writer.Name,
                    Performer = s.SongPerformers.Select(p => $"{p.Performer.FirstName} {p.Performer.LastName}").FirstOrDefault(),
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(SongXmlDto[]), new XmlRootAttribute("Songs"));
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), songsDto, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}