using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.DataProcessor.ExportDto
{
    public class ExportMovieDto
    {
        public string MovieName { get; set; }

        public string Rating { get; set; }

        public string TotalIncomes { get; set; }

        public ICollection<ExportCustomerDto> Customers{ get; set; }
    }
}
