using Axity.Security.Core.Dto.UseCaseResponses;
using Axity.Security.Core.Interfaces;

namespace Axity.Security.Core.Dto.UseCaseRequests
{
    public class AddRoleUserRequest : IUseCaseRequest<AddRoleUserResponse>
    {
        public string UserName { get; }
        public string[] Roles { get; }

        public AddRoleUserRequest(string userName, string[] roles)
        {
            UserName = userName;
            Roles = roles;
        }
    }
}
