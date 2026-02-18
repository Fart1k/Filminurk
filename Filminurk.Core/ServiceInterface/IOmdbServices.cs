using Filminurk.Core.Domain;
using Filminurk.Core.Dto.OmdbDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.ServiceInterface
{
    public interface IOmdbServices
    {
        Task<OmdbRootDTO> OmdbRootSearchResult(string title);

        Movie CreateMovieFromOmdb(OmdbImportMovieDTO dto);
    }
}
