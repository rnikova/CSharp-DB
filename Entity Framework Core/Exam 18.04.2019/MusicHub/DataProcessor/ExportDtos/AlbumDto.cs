using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.DataProcessor.ExportDtos
{
    public class AlbumDto
    {
        [StringLength(40, MinimumLength = 3)]
        public string AlbumName { get; set; }

        public string ReleaseDate { get; set; }

        public string ProducerName { get; set; }

        public ICollection<SongDto> Songs { get; set; }

        public string AlbumPrice { get; set; }
    }
}
