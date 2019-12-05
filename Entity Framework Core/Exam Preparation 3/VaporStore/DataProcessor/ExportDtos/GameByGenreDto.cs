using System;
using System.Collections.Generic;
using System.Text;

namespace VaporStore.DataProcessor.ExportDtos
{
    public class GameByGenreDto
    {
        public int Id { get; set; }

        public string Genre { get; set; }

        public ICollection<GameDto> Games { get; set; }

        public int TotalPlayers { get; set; }
    }
}
