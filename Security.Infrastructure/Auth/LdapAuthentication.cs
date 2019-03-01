using Axity.Security.Core.Domain.LDAP;
using Axity.Security.Infrastructure.Identity;
using Novell.Directory.Ldap;
using System;
using System.Linq;

namespace Axity.Security.Infrastructure.Auth
{
    /// <summary>
    /// A class that provides password verification against an LDAP store by attempting to bind.
    /// </summary>
    public class LdapAuthentication : IDisposable
    {
        private readonly LdapAuthenticationOptions _options;
        private bool _isDisposed = false;
        private int _ldapPort;
        private int _searchScope;
        private int _ldapVersion;

        /// <summary>
        /// Initializes a new instance with the the given options.
        /// </summary>
        /// <param name="options"></param>
        public LdapAuthentication(LdapAuthenticationOptions options)
        {
            _options = options;
            _ldapPort = LdapConnection.DEFAULT_PORT;
            _searchScope = LdapConnection.SCOPE_SUB;
            _ldapVersion = LdapConnection.Ldap_V3;
        }


        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
        }

        /// <summary>
        /// Gets a value that indicates if the password for the user identified by the given DN is valid.
        /// </summary>
        /// <param name="samAccountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LDAPIdentityResult ValidatePassword(string samAccountName, string password)
        {
            LDAPIdentityResult identityResult = new LDAPIdentityResult();

            try
            {
                if (_options == null)
                    throw new InvalidOperationException("The configuration has not been established.");
                if (string.IsNullOrEmpty(_options.LDAPHost))
                    throw new InvalidOperationException("The LDAP Host cannot be empty or null.");
                if (string.IsNullOrEmpty(_options.LDAPAccountDN))
                    throw new InvalidOperationException("The LDAP Account service cannot be empty or null.");
                if (string.IsNullOrEmpty(_options.LDAPPasswordDN))
                    throw new InvalidOperationException("The LDAP Password Account service cannot be empty or null.");
                if (string.IsNullOrEmpty(_options.LDAPSearchBase))
                    throw new InvalidOperationException("The LDAP Search Base Filter cannot be empty or null.");

                string searchFilter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + samAccountName + "))";
                string userdistinguishName = string.Empty;
                string nameUser = string.Empty, mail = string.Empty;

                using (LdapConnection ldapConnection = new LdapConnection())
                {
                    ldapConnection.Connect(_options.LDAPHost, this._ldapPort);
                    ldapConnection.Bind(_ldapVersion, _options.LDAPAccountDN, _options.LDAPPasswordDN);

                    LdapSearchResults searchResults = ldapConnection.Search(_options.LDAPSearchBase, _searchScope, searchFilter, null, false);
                    while (searchResults.hasMore())
                    {
                        LdapEntry nextEntry = null;
                        try
                        {
                            nextEntry = searchResults.next();
                        }
                        catch (LdapException e)
                        {
                            identityResult.SetError(e.ResultCode.ToString(), e.LdapErrorMessage);
                            break;
                        }

                        userdistinguishName = nextEntry.DN;
                        LdapAttributeSet attributeSet = nextEntry.getAttributeSet();
                        nameUser = attributeSet.getAttribute("name").StringValue;
                        mail = attributeSet.getAttribute("mail").StringValue;
                    }
                    ldapConnection.Disconnect();

                    if (string.IsNullOrEmpty(userdistinguishName))
                        throw new InvalidOperationException("Invalid username.");
                    try
                    {
                        LdapConnection conn = new LdapConnection();
                        conn.Connect(_options.LDAPHost, _ldapPort);
                        conn.Bind(userdistinguishName, password);
                        conn.Disconnect();

                        identityResult.SetData(nameUser, string.Empty, mail, samAccountName);
                    }
                    catch (LdapException e)
                    {
                        identityResult.SetError(e.ResultCode.ToString(), e.LdapErrorMessage);
                    }
                }
            }
            catch (LdapException e)
            {
                identityResult.SetError(e.ResultCode.ToString(), e.LdapErrorMessage);
            }
            catch (Exception ex)
            {
                identityResult.SetError(ex.HResult.ToString(), ex.Message);
            }
            return identityResult;
        }
    }
}
