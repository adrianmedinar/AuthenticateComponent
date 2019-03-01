using System;
using System.Collections.Generic;
using System.Text;

namespace Axity.Security.Infrastructure.Identity
{
    public class LdapAuthenticationOptions
    {
        /// <summary>
        /// Gets or sets the LDAP host name.
        /// </summary>
        public string LDAPHost { get; set; }
        /// <summary>
        /// Distinguish Name of Account Service LDAP
        /// </summary>
        public string LDAPAccountDN { get; set; }
        /// <summary>
        /// Password account service LDAP
        /// </summary>
        public string LDAPPasswordDN { get; set; }
        /// <summary>
        /// LDAP Search Base Filter
        /// </summary>
        public string LDAPSearchBase { get; set; }
     }
}
