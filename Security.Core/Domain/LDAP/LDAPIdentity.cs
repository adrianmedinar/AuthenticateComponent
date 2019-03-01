using System;
using System.Collections.Generic;
using System.Text;

namespace Axity.Security.Core.Domain.LDAP
{
    public class LDAPIdentity
    {
        public string FirstName { get; protected set; }
        public string LasttName { get; protected set; }
        public string Email { get; protected set; }
        public string UserName { get; protected set; }

        public LDAPIdentity(string firstName, string lastName, string mail, string userName)
        {
            FirstName = firstName;
            LasttName = lastName;
            Email = mail;
            UserName = userName;
        }
    }
}
