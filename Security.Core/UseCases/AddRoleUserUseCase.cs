using System.Linq;
using System.Threading.Tasks;
using Axity.Security.Core.Dto.UseCaseRequests;
using Axity.Security.Core.Dto.UseCaseResponses;
using Axity.Security.Core.Interfaces;
using Axity.Security.Core.Interfaces.Gateways.Repositories;
using Axity.Security.Core.Interfaces.UseCases;

namespace Axity.Security.Core.UseCases
{
    public sealed class AddRoleUserUseCase : IAddRoleUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public AddRoleUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(AddRoleUserRequest message, IOutputPort<AddRoleUserResponse> outputPort)
        {
            var response = await _userRepository.AddRolesToUser(message.UserName, message.Roles);
            outputPort.Handle(response.Success ? new AddRoleUserResponse(response.Id, true) : new AddRoleUserResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
