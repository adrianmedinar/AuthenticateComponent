using System.Collections.Generic;
using Axity.Security.Core.Interfaces;

namespace Axity.Security.Core.Dto.UseCaseResponses
{
    public class LoginResponse : UseCaseResponseMessage
    {
        public UserData UserData { get; }
        public AccessToken AccessToken { get; }
        public string RefreshToken { get; }
        public IEnumerable<Error> Errors { get; }

        public LoginResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public LoginResponse(UserData userData, AccessToken accessToken, string refreshToken, bool success = false, string message = null) : base(success, message)
        {
            UserData = userData;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
