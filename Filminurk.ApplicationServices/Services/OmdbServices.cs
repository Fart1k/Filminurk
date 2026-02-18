using Filminurk.Core.Domain;
using Filminurk.Core.Dto.OmdbDTOs;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using System.Text.Json;

namespace Filminurk.ApplicationServices.Services
{
    public class OmdbServices : IOmdbServices
    {
        private readonly FilminurkTARpe24Context _context;
        public OmdbServices(FilminurkTARpe24Context context)
        {
            _context = context;
        }

        public async Task<OmdbRootDTO> OmdbRootSearchResult(string title)
        {
            string apiKey = Filminurk.Data.Environment.omdbkey;
            var baseUrl = "http://www.omdbapi.com/";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{baseUrl}?apikey={apiKey}&t={title}");
                var jsonResponse = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<OmdbRootDTO>(jsonResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public Movie CreateMovieFromOmdb(OmdbImportMovieDTO dto)
        {
            Movie movie = new();
            movie.ID = (Guid)dto.Id;
            movie.Title = dto.Title;
            movie.FirstPublished = (DateOnly)dto.FirstPublished;
            movie.MovieGenre = dto.Genre;
            movie.Director = dto.Director;
            movie.Actors = dto.Actors;
            movie.Description = dto.Description;
            movie.CurrentRating = dto.CurrentRating;

            movie.EntryCreatedAt = DateTime.Now;
            movie.EntryModifiedAt = DateTime.Now;

            _context.Movies.Add(movie);
            _context.SaveChangesAsync();
            return movie;
        }
    }
}
