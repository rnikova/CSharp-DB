using System;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.DataProcessor.ImportDtos
{
    public class AlbumDto
    {
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }

        public string ReleaseDate { get; set; }
    }
}
