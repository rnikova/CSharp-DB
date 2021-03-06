﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
	public class Game
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(typeof(decimal), "0.00", "79228162514264337593543950335M")]
        public decimal Price { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public int DeveloperId { get; set; }

        [Required]
        public Developer Developer { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        public Genre Genre { get; set; }

        public ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();

        public ICollection<GameTag> GameTags { get; set; } = new HashSet<GameTag>();
    }
}
