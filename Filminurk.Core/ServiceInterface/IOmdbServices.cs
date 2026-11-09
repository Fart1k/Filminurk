using Filminurk.Core.Domain;
using Filminurk.Core.Dto.OmdbDTOs;

namespace Filminurk.Core.ServiceInterface
{
    public interface IOmdbServices
    {
        Task<OmdbRootDTO> OmdbRootSearchResult(string title);

        Task<Movie> CreateMovieFromOmdb(OmdbImportMovieDTO dto);
    }
}
