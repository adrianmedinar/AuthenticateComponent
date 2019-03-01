using System;
using System.Collections.Generic;
using System.Text;

namespace Axity.Security.Core.Domain.LDAP
{
    public class LDAPIdentityResult
    {
        public List<LDAPError> Errors { get; protected set; }
        public LDAPIdentity Success { get; protected set; }
        public bool Succeeded { get; protected set; } = false;
 

        public LDAPIdentityResult()
        {
            Errors = new List<LDAPError>();
        }

        public void SetError(string code, string message)
        {
            Errors.Add(new LDAPError(code, message));
        }

        public void SetData(string firstName, string lasttName, string email, string userName)
        {
            Success = new LDAPIdentity(firstName, lasttName, email, userName);
            Succeeded = true;
        }
    }
}
