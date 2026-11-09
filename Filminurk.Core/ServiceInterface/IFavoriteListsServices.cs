using Filminurk.Core.Domain;
using Filminurk.Core.Dto;

namespace Filminurk.Core.ServiceInterface
{
    public interface IFavoriteListsServices
    {
        Task<FavoriteList> DetailsAsync(Guid id);

        Task<FavoriteList> Create(FavoriteListDTO dto /*, List<Movie> selectedMovies */);

        Task<FavoriteList> Update(FavoriteListDTO privatedList, string typeOfMethod);
    }
}
