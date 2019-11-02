using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Town
    {
        [Key]
        public int TownId { get; set; }

        public string Name { get; set; }

        [ForeignKey("Country")]
        public Country CountryId { get; set; }

        public Country Country { get; set; }
    }
}
