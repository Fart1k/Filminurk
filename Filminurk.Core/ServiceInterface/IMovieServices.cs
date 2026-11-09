using Filminurk.Core.Domain;
using Filminurk.Core.Dto;

namespace Filminurk.Core.ServiceInterface
{
    public interface IMovieServices
    {
        Task<Movie> Create(MoviesDTO dto);
        Task<Movie> Update(MoviesDTO dto);
        Task<Movie> Delete(Guid id);
        Task<Movie> DetailsAsync(Guid id);
    }
}