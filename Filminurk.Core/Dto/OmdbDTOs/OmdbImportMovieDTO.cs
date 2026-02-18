using Filminurk.Core.Domain;

namespace Filminurk.Core.Dto.OmdbDTOs
{
    public class OmdbImportMovieDTO
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public DateOnly? FirstPublished { get; set; }
        public Genre Genre { get; set; }
        public string Director { get; set; }
        public List<string>? Actors { get; set; }
        public string? Description { get; set; }
        public double? CurrentRating { get; set; }
        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
