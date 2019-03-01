using System.Threading.Tasks;
using Axity.Security.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Axity.Security.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        // Add additional profile data for application users by adding properties to this class
    }

    public class AppRole : IdentityRole
    {

    }
}
