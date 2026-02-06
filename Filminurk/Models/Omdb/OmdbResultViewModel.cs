using Filminurk.Core.Domain;
using System.Reflection.Metadata.Ecma335;

namespace Filminurk.Models.Omdb
{
    public class OmdbResultViewModel
    {
        public string Title { get; set; }
        public string Released { get; set; }
        public Genre Genre { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string Description { get; set; }
        public string imdbRating { get; set; }
    }
}
