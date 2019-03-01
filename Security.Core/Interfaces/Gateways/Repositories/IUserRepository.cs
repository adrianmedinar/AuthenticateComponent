using System.Threading.Tasks;
using Axity.Security.Core.Domain.Entities;
using Axity.Security.Core.Domain.LDAP;
using Axity.Security.Core.Dto.GatewayResponses.Repositories;

namespace Axity.Security.Core.Interfaces.Gateways.Repositories
{
    public interface IUserRepository  : IRepository<User>
    {
        Task<CreateUserResponse> Create(string firstName, string lastName, string email, string userName, string password, string[] roles);
        Task<User> FindByName(string userName);
        Task<bool> CheckPassword(User user, string password);
        Task<AddRoleToUserReponse> AddRolesToUser(string userName, string[] roles);
        Task<string[]> GetUserRoles(string userName);
        Task<LDAPIdentityResult> CheckPasswordLdap(string userName, string password);
    }
}
