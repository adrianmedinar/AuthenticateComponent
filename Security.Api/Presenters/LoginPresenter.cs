using System.Net;
using Axity.Security.Core.Dto.UseCaseResponses;
using Axity.Security.Core.Interfaces;
using Axity.Security.Api.Serialization;

namespace Axity.Security.Api.Presenters
{
    public sealed class LoginPresenter : IOutputPort<LoginResponse>
    {
        public JsonContentResult ContentResult { get; }

        public LoginPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(LoginResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.Unauthorized);
            ContentResult.Content = response.Success ? JsonSerializer.SerializeObject(new Models.Response.LoginResponse(response.UserData, response.AccessToken, response.RefreshToken)) : JsonSerializer.SerializeObject(response.Errors);
        }
    }
}
