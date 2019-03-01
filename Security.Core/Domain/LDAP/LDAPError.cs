using System;
using System.Collections.Generic;
using System.Text;

namespace Axity.Security.Core.Domain.LDAP
{
    public class LDAPError
    {
        public string Code { get; protected set; }
        public string Description { get; protected set; }

        public LDAPError()
        {
            Code = null;
            Description = null;
        }
        public LDAPError(string code, string message)
        {
            Code = code;
            Description = message;
        }
    }
}
