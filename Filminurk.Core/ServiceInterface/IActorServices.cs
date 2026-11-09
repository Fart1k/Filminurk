using Filminurk.Core.Domain;
using Filminurk.Core.Dto;

namespace Filminurk.Core.ServiceInterface
{
    public interface IActorServices
    {
        Task<Actors> Create(ActorsDTO dto);
        Task<Actors> Update(ActorsDTO dto);
        Task<Actors> Delete(Guid id);
        Task<Actors> DetailsAsync(Guid id);
    }
}
