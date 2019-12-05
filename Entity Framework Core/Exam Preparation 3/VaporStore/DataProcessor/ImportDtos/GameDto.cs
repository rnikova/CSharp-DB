using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDtos
{
    public class GameDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0.00, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string ReleaseDate { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public string Genre { get; set; }

        public ICollection<string> Tags { get; set; }
    }
}
