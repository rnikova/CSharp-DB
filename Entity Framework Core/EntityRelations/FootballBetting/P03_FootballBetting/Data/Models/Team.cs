﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        public string Name { get; set; }

        public string LogoUrl { get; set; }

        [MaxLength(3)]
        public string Initials { get; set; }

        public decimal Budget { get; set; }

        [ForeignKey("Color")]
        public int PrimaryKitColorId { get; set; }

        public Color PrimaryKitColor { get; set; }
        
        [ForeignKey("Color")]
        public int SecondaryKitColorId { get; set; }

        public Color SecondaryKitColor { get; set; }

        [ForeignKey("Town")]
        public int TownId { get; set; }

        public Town Town { get; set; }
    }
}
