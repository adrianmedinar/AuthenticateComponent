

namespace Axity.Security.Core.Dto
{
    public sealed class UserData
    {
        public string FullUserName { get; }
        public string[] Roles { get; }

        public UserData(string name, string[] roles)
        {
            FullUserName = name;
            Roles = roles;
        }
    }
}
