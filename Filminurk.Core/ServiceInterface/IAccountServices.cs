using Filminurk.Core.Domain;
using Filminurk.Core.Dto.AccountsDTOs;

namespace Filminurk.Core.ServiceInterface
{
    public interface IAccountServices
    {
        Task<ApplicationUser> Register(ApplicationUsersDTOs userDTO);
        Task<ApplicationUser> Login(LoginDTO userDTO);
    }
}
