using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Axity.Security.Core.Dto.UseCaseRequests;
using Axity.Security.Core.Interfaces.UseCases;
using Axity.Security.Api.Models.Settings;
using Axity.Security.Api.Presenters;

using Axity.Security.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Axity.Security.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUseCase _loginUseCase;
        private readonly LoginPresenter _loginPresenter;
        private readonly IExchangeRefreshTokenUseCase _exchangeRefreshTokenUseCase;
        private readonly ExchangeRefreshTokenPresenter _exchangeRefreshTokenPresenter;
        private readonly AuthSettings _authSettings;
        
        public AuthController(ILoginUseCase loginUseCase, 
                              LoginPresenter loginPresenter, 
                              IExchangeRefreshTokenUseCase exchangeRefreshTokenUseCase, 
                              ExchangeRefreshTokenPresenter exchangeRefreshTokenPresenter,
                              IOptions<AuthSettings> authSettings)
        {
            _loginUseCase = loginUseCase;
            _loginPresenter = loginPresenter;
            _exchangeRefreshTokenUseCase = exchangeRefreshTokenUseCase;
            _exchangeRefreshTokenPresenter = exchangeRefreshTokenPresenter;
            _authSettings = authSettings.Value;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Models.Request.LoginRequest request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _loginUseCase.Handle(new LoginRequest(request.UserName, request.Password, Request.HttpContext.Connection.RemoteIpAddress?.ToString()), _loginPresenter);
            return _loginPresenter.ContentResult;
        }

        [HttpPost("loginAD")]
        public async Task<ActionResult> LoginAD([FromBody] Models.Request.LoginRequest request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            await _loginUseCase.Handle(new LoginRequest(
                                            request.UserName, 
                                            request.Password, 
                                            Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                                            true, true), _loginPresenter);
            return _loginPresenter.ContentResult;
        }

        // POST api/auth/refreshtoken
        [HttpPost("refreshtoken")]
        public async Task<ActionResult> RefreshToken([FromBody] Models.Request.ExchangeRefreshTokenRequest request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState);}
            await _exchangeRefreshTokenUseCase.Handle(new ExchangeRefreshTokenRequest(request.AccessToken, request.RefreshToken, _authSettings.SecretKey), _exchangeRefreshTokenPresenter);
            return _exchangeRefreshTokenPresenter.ContentResult;
        }
    }
}
