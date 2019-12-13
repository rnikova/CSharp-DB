using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.DataProcessor.ExportDto
{
    public class ExportAuthorDto
    {
        public string AuthorName { get; set; }

        public ICollection<ExportBookDto> Books { get; set; }
    }
}
