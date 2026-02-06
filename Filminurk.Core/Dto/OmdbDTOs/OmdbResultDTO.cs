using Filminurk.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.Dto.OmdbDTOs
{
    public class OmdbResultDTO
    {
        public string Title { get; set; }
        public string Released { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string Description { get; set; }
        public string imdbRating { get; set; }
    }
}
