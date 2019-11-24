using System;
using System.ComponentModel.DataAnnotations;

namespace Cinema.DataProcessor.ImportDto
{
    public class MovieDto
    {
        [StringLength(20, MinimumLength = 3)]
        public string Title { get; set; }

        public string Genre { get; set; }

        public TimeSpan Duration { get; set; }

        [Range(1, 10)]
        public double Rating { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Director { get; set; }
    }
}
