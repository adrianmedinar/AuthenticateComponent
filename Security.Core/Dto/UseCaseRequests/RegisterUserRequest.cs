using Axity.Security.Core.Dto.UseCaseResponses;
using Axity.Security.Core.Interfaces;

namespace Axity.Security.Core.Dto.UseCaseRequests
{
    public class RegisterUserRequest : IUseCaseRequest<RegisterUserResponse>
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string UserName { get; }
        public string Password { get; }
        public string[] Roles { get; }

        public RegisterUserRequest(string firstName, string lastName, string email, string userName, string password, string[] roles)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
            Password = password;
            Roles = roles;
        }
    }
}
