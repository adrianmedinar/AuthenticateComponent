

namespace Axity.Security.Api.Models.Request
{
    public class AddRolesUserRequest
    {
        public string UserName { get; set; }
        public string[] Roles { get; set; }
    }
}
