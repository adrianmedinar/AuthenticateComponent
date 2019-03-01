using Axity.Security.Core.Dto.UseCaseResponses;
using Axity.Security.Core.Interfaces;

namespace Axity.Security.Core.Dto.UseCaseRequests
{
    public class LoginRequest : IUseCaseRequest<LoginResponse>
    {
        public string UserName { get; }
        public string Password { get; }
        public string RemoteIpAddress { get; }
        public bool HandleLDAP { get; }
        public bool ManageRol { get; }

        public LoginRequest(string userName, string password, string remoteIpAddress, bool handleLDAP = false, bool manageRol = false)
        {
            UserName = userName;
            Password = password;
            RemoteIpAddress = remoteIpAddress;
            HandleLDAP = handleLDAP;
            ManageRol = manageRol;
        }
    }
}
