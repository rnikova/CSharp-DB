using System.ComponentModel.DataAnnotations;

namespace Cinema.DataProcessor.ImportDto
{
   public class ImportMovieDto
    {
        [MinLength(3)]
        [MaxLength(20)]
        public string Title { get; set; }

        public string Genre { get; set; }

        public string Duration { get; set; }

        [Range(1, 10)]
        public double Rating { get; set; }

        [MinLength(3)]
        [MaxLength(20)]
        public string Director { get; set; }
    }
}
