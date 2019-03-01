using System.Threading.Tasks;
using Axity.Security.Core.Dto;
using Axity.Security.Core.Dto.UseCaseRequests;
using Axity.Security.Core.Dto.UseCaseResponses;
using Axity.Security.Core.Interfaces;
using Axity.Security.Core.Interfaces.Gateways.Repositories;
using Axity.Security.Core.Interfaces.Services;
using Axity.Security.Core.Interfaces.UseCases;


namespace Axity.Security.Core.UseCases
{
    public sealed class LoginUseCase : ILoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;

        public LoginUseCase(IUserRepository userRepository, IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
        }

        public async Task<bool> Handle(LoginRequest message, IOutputPort<LoginResponse> outputPort)
        {
            if (!string.IsNullOrEmpty(message.UserName) && !string.IsNullOrEmpty(message.Password))
            {
                // ensure we have a user with the given user name
                if (message.HandleLDAP)
                {
                    var result = await _userRepository.CheckPasswordLdap(message.UserName, message.Password);
                    if (result.Succeeded)
                    {
                        var dataIdentity = result.Success;
                        var userExists = await _userRepository.FindByName(message.UserName);
                        if (userExists == null)
                        {
                            var resultCreate = await _userRepository.Create(dataIdentity.FirstName, dataIdentity.LasttName, dataIdentity.Email, dataIdentity.UserName, dataIdentity.UserName, new string[] { });
                            if (!resultCreate.Success)
                            {
                                outputPort.Handle(new LoginResponse(resultCreate.Errors, resultCreate.Success));
                                return false;
                            }
                            userExists = await _userRepository.FindByName(message.UserName);
                        }
                        await GenerateTokenOrRefresh(message, outputPort, userExists);
                        return true;
                    }
                    else
                    {
                        outputPort.Handle(new LoginResponse(new[] { new Error("login_failure", "Invalid username or password.") }));
                        return false;
                    }
                }
                var user = await _userRepository.FindByName(message.UserName);
                if (user != null)
                {
                    // validate password
                    if (await _userRepository.CheckPassword(user, message.Password))
                    {
                        // generate or get refresh token
                        await GenerateTokenOrRefresh(message, outputPort, user);
                        return true;
                    }
                }
            }
            outputPort.Handle(new LoginResponse(new[] { new Error("login_failure", "Invalid username or password.") }));
            return false;
        }

        private async Task GenerateTokenOrRefresh(LoginRequest message, IOutputPort<LoginResponse> outputPort, Domain.Entities.User user)
        {
            string refreshToken;
            if (user.HasValidToken())
                refreshToken = user.GetActiveTokenUser;
            else
            {
                refreshToken = _tokenFactory.GenerateToken();
                user.AddRefreshToken(refreshToken, user.Id, message.RemoteIpAddress, _jwtFactory.LifeTimeToken());
                await _userRepository.Update(user);
            }

            string fullUserName = string.Format("{0} {1}", user.FirstName, user.LastName);
            var roles = await _userRepository.GetUserRoles(user.UserName);
            UserData userData = new UserData(fullUserName, roles);
            // generate response
            outputPort.Handle(new LoginResponse(userData, await _jwtFactory.GenerateEncodedToken(user.IdentityId, user.UserName), refreshToken, true));
        }
    }
}
