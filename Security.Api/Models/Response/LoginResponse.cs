 

using Axity.Security.Core.Dto;

namespace Axity.Security.Api.Models.Response
{
    public class LoginResponse
    {
        public UserData UserData { get; }
        public AccessToken AccessToken { get; }
        public string RefreshToken { get; }

        public LoginResponse(UserData userData, AccessToken accessToken, string refreshToken)
        {
            UserData = userData;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
