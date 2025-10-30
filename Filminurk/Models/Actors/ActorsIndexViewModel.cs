using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Models.Actors
{
    public class ActorsIndexViewModel : Controller
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}
