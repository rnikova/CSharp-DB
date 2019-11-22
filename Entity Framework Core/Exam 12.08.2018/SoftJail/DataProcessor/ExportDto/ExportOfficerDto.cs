using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ExportDto
{
    public class ExportOfficerDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string OfficerName { get; set; }

        public string Department { get; set; }
    }
}
