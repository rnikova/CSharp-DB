using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.DataProcessor.ExportDtos
{
    public class AlbumDto
    {
        [MinLength(3)]
        [MaxLength(40)]
        public string AlbumName { get; set; }

        public string ReleaseDate { get; set; }

        public string ProducerName { get; set; }

        public ICollection<SongDto> Songs { get; set; }

        public string AlbumPrice { get; set; }
    }
}
