using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.DataProcessor.ExportDto
{
    public class MovieDto
    {
        [StringLength(20, MinimumLength = 3)]
        public string MovieName { get; set; }

        public string Rating { get; set; }

        public string TotalIncomes { get; set; }

        public ICollection<CustomerDto> Customers { get; set; }
    }
}
