using System.Collections.Generic;

namespace Axity.Security.Core.Dto.GatewayResponses.Repositories
{
    public sealed class AddRoleToUserReponse : BaseGatewayResponse
    {
        public string Id { get; }
        public AddRoleToUserReponse(string id, bool success = false, IEnumerable<Error> errors = null) : base(success, errors)
        {
            Id = id;
        }
    }
}
