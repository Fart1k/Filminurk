using Filminurk.Core.Domain;

namespace Filminurk.Core.Dto
{
    public class ActorsDTO
    {
        public Guid? ActorID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? NickName { get; set; }
        public List<string>? MoviesActedFor { get; set; }
        public Guid? PortraitID { get; set; }
        public int Age { get; set; }
        public required string Gender { get; set; }
        public Region? ActorRegion { get; set; }
        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
