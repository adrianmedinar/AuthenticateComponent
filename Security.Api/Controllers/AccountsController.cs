using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Axity.Security.Core.Dto.UseCaseRequests;
using Axity.Security.Core.Interfaces.UseCases;
using Axity.Security.Api.Presenters;

namespace Axity.Security.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUserUseCase;
        private readonly RegisterUserPresenter _registerUserPresenter;
        private readonly IAddRoleUserUseCase _addRoleUserUseCase;
        private readonly AddRoleUserPresenter _addRoleUserPresenter;

        public AccountsController(IRegisterUserUseCase registerUserUseCase, RegisterUserPresenter registerUserPresenter, IAddRoleUserUseCase addRoleUserUseCase, AddRoleUserPresenter addRoleUserPresenter)
        {
            _registerUserUseCase = registerUserUseCase;
            _registerUserPresenter = registerUserPresenter;
            _addRoleUserUseCase = addRoleUserUseCase;
            _addRoleUserPresenter = addRoleUserPresenter;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Models.Request.RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _registerUserUseCase.Handle(new RegisterUserRequest(request.FirstName, 
                request.LastName, 
                request.Email, 
                request.UserName, 
                request.Password, 
                request.Roles), _registerUserPresenter);
            return _registerUserPresenter.ContentResult;
        }

        [HttpPost("AddRole")]
        public async Task<ActionResult> AddRoleToUser([FromBody] Models.Request.AddRolesUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _addRoleUserUseCase.Handle(new AddRoleUserRequest(request.UserName, request.Roles), _addRoleUserPresenter);
            return _addRoleUserPresenter.ContentResult;
        }
    }
}
