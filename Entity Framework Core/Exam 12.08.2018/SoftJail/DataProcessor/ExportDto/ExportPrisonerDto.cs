using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ExportDto
{
    public class ExportPrisonerDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(1, 1000)]
        public int CellNumber { get; set; }

        public ICollection<ExportOfficerDto> Officers { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal TotalOfficerSalary { get; set; }
    }
}
