using System.ComponentModel.DataAnnotations;

namespace MusicHub.DataProcessor.ExportDtos
{
    public class SongDto
    {
        [StringLength(20, MinimumLength = 3)]
        public string SongName { get; set; }

        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        public string Price { get; set; }

        public string Writer { get; set; }
    }
}
