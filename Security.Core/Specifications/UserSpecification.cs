using Axity.Security.Core.Domain.Entities;

namespace Axity.Security.Core.Specifications
{
    public sealed class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(string identityId) : base(u => u.IdentityId==identityId)
        {
            AddInclude(u => u.RefreshTokens);
        }
    }
}
