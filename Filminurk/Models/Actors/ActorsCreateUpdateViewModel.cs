using Filminurk.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Models.Actors
{
    public class ActorsCreateUpdateViewModel : Controller
    {
        public Guid? ActorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? NickName { get; set; }
        public List<string>? MoviesActedFor { get; set; }
        public Guid? PortraitID { get; set; }

        //

        public int Age { get; set; }
        public string Gender { get; set; }
        public Region? ActorRegion { get; set; }

        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
