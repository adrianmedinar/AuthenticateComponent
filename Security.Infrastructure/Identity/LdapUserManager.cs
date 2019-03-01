using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Axity.Security.Infrastructure.Auth;
using Axity.Security.Core.Domain.LDAP;

namespace Axity.Security.Infrastructure.Identity
{
    public class LdapUserManager
    {
        private readonly LdapAuthenticationOptions _ldapOptions;

        /// <summary>
        /// Initializes an instance.
        /// </summary>
        /// <param name="ldapOptions"></param>
        public LdapUserManager(IOptions<LdapAuthenticationOptions> ldapOptions)
        {
            _ldapOptions = ldapOptions.Value;
        }

        /// <summary>
        /// Checks the given password agains the configured LDAP server.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<LDAPIdentityResult> CheckPasswordAsync(string userName, string password)
        {
            using (var auth = new LdapAuthentication(_ldapOptions))
            {
                var result = auth.ValidatePassword(userName, password);
                return await Task.FromResult(result);
            }
        }
    }
}
