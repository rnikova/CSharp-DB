using System;
using System.Collections.Generic;
using System.Text;

namespace VaporStore.DataProcessor.ExportDtos
{
    public class ExportGenreDto
    {
        //id, genre name, games and total players (total purchase count
        public int Id { get; set; }

        public string Genre { get; set; }

        public ICollection<ExportGameDto> Games { get; set; }

        public int TotalPlayers { get; set; }
    }
}
