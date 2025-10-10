using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Models.Movies
{
    public class MoviesIndexViewModel : Controller
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public DateOnly FirstPublished { get; set; }
        public double? CurrentRating { get; set; }

        public string? Warnings { get; set; }
    }
}
