using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.ImportDtos
{
    public class ImportCardsDto
    {
        [Required]
        [RegularExpression(@"^[\d]{4}\s+[\d]{4}\s+[\d]{4}\s+[\d]{4}$")]
        public string Number { get; set; }

        [Required]
        [StringLength(3)]
        public string CVC { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
