using System.Net;
using Axity.Security.Core.Dto.UseCaseResponses;
using Axity.Security.Core.Interfaces;
using Axity.Security.Api.Serialization;

namespace Axity.Security.Api.Presenters
{
    public sealed class AddRoleUserPresenter : IOutputPort<AddRoleUserResponse>
    {
        public JsonContentResult ContentResult { get; }

        public AddRoleUserPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(AddRoleUserResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = JsonSerializer.SerializeObject(response);
        }
    }
}
