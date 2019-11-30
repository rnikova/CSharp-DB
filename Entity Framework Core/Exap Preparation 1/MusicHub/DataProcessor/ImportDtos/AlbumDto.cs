﻿using System.ComponentModel.DataAnnotations;

namespace MusicHub.DataProcessor.ImportDtos
{
    public class AlbumDto
    {
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }

        public string ReleaseDate { get; set; }
    }
}
