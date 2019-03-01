using System.Linq;
using System.Threading.Tasks;
using Axity.Security.Core.Dto.UseCaseRequests;
using Axity.Security.Core.Dto.UseCaseResponses;
using Axity.Security.Core.Interfaces;
using Axity.Security.Core.Interfaces.Gateways.Repositories;
using Axity.Security.Core.Interfaces.UseCases;

namespace Axity.Security.Core.UseCases
{
    public sealed class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(RegisterUserRequest message, IOutputPort<RegisterUserResponse> outputPort)
        {
            var response = await _userRepository.Create(message.FirstName, message.LastName,message.Email, message.UserName, message.Password, message.Roles);
            outputPort.Handle(response.Success ? new RegisterUserResponse(response.Id, true) : new RegisterUserResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
