using System.Net;
using Axity.Security.Core.Dto.UseCaseResponses;
using Axity.Security.Core.Interfaces;
using Axity.Security.Api.Serialization;

namespace Axity.Security.Api.Presenters
{
    public sealed class RegisterUserPresenter : IOutputPort<RegisterUserResponse>
    {
        public JsonContentResult ContentResult { get; }

        public RegisterUserPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(RegisterUserResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = JsonSerializer.SerializeObject(response);
        }
    }
}
