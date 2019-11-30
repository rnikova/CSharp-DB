using System.ComponentModel.DataAnnotations;

namespace MusicHub.DataProcessor.ExportDtos
{
    public class SongDto
    {
        [MinLength(3)]
        [MaxLength(20)]
        public string SongName { get; set; }

        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        public string Price { get; set; }

        public string Writer { get; set; }
    }
}
