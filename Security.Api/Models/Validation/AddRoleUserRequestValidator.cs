using FluentValidation;
using Axity.Security.Api.Models.Request;

namespace Axity.Security.Api.Models.Validation
{
    public class AddRoleUserRequestValidator : AbstractValidator<AddRolesUserRequest>
    {
        public AddRoleUserRequestValidator()
        {
            RuleFor(x => x.UserName).Length(3, 255);
            RuleFor(x => x.Roles).NotNull();
            RuleFor(x => x.Roles).NotEmpty();
            RuleForEach(x => x.Roles).NotEmpty();
        }
    }
}
