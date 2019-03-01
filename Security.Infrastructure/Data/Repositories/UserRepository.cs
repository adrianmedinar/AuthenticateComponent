using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Axity.Security.Core.Domain.Entities;
using Axity.Security.Core.Dto;
using Axity.Security.Core.Dto.GatewayResponses.Repositories;
using Axity.Security.Core.Interfaces.Gateways.Repositories;
using Axity.Security.Core.Specifications;
using Axity.Security.Infrastructure.Identity;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Axity.Security.Core.Domain.LDAP;

namespace Axity.Security.Infrastructure.Data.Repositories
{
    internal sealed class UserRepository : EfRepository<User>, IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly LdapUserManager _ldapUserManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;


        public UserRepository(UserManager<AppUser> userManager, IMapper mapper, IOptions<LdapAuthenticationOptions> options, AppDbContext appDbContext) : base(appDbContext)
        {
            _userManager = userManager;
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(appDbContext), null, null, null, null);
            _ldapUserManager = new LdapUserManager(options);
            _mapper = mapper;
        }

        public async Task<CreateUserResponse> Create(string firstName, string lastName, string email, string userName, string password, string[] roles)
        {
            roles = roles.Select(r => r.ToUpperInvariant()).ToArray();
            for (int c = 0; c < roles.Length; c++)
            {
                string rol = roles[c];
                bool existRol = await _roleManager.RoleExistsAsync(rol);
                if (!existRol)
                {
                    var appRol = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = rol, ConcurrencyStamp = Guid.NewGuid().ToString() };
                    var roleResult = await _roleManager.CreateAsync(appRol);
                    if (!roleResult.Succeeded)
                        return new CreateUserResponse(rol, false, roleResult.Errors.Select(e => new Error(e.Code, e.Description)));
                }
            }

            var appUser = new AppUser {Email = email, UserName = userName};
            var identityResult = await _userManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded) return new CreateUserResponse(appUser.Id, false,identityResult.Errors.Select(e => new Error(e.Code, e.Description)));

            if (roles.Length > 0)
            {
                var rolesAddResult = await _userManager.AddToRolesAsync(appUser, roles);
                if (!rolesAddResult.Succeeded) return new CreateUserResponse(appUser.Id, false, rolesAddResult.Errors.Select(e => new Error(e.Code, e.Description)));
            }

            var user = new User(firstName, lastName, appUser.Id, appUser.UserName);
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();

            return new CreateUserResponse(appUser.Id, identityResult.Succeeded, identityResult.Succeeded ? null : identityResult.Errors.Select(e => new Error(e.Code, e.Description)));
        }

        public async Task<User> FindByName(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            return appUser == null ? null : _mapper.Map(appUser, await GetSingleBySpec(new UserSpecification(appUser.Id)));
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(_mapper.Map<AppUser>(user), password);
        }

        public async Task<AddRoleToUserReponse> AddRolesToUser(string userName, string[] roles)
        {
            roles = roles.Select(r => r.ToUpperInvariant()).ToArray();
            for (int c = 0; c < roles.Length; c++)
            {
                string rol = roles[c];
                bool existRol = await _roleManager.RoleExistsAsync(rol);
                if (!existRol)
                {
                    var appRol = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = rol, ConcurrencyStamp = Guid.NewGuid().ToString() };
                    var roleResult = await _roleManager.CreateAsync(appRol);
                    if (!roleResult.Succeeded)
                        return new AddRoleToUserReponse(rol, false, roleResult.Errors.Select(e => new Error(e.Code, e.Description)));
                }
            }
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null) return new AddRoleToUserReponse(userName, false,new List<Error> { new Error("user does not exists", "user does not exists") });
            var rolesAddResult = await _userManager.AddToRolesAsync(appUser, roles);
            if (!rolesAddResult.Succeeded) return new AddRoleToUserReponse(appUser.Id, false, rolesAddResult.Errors.Select(e => new Error(e.Code, e.Description)));

            return new AddRoleToUserReponse(appUser.Id, rolesAddResult.Succeeded, rolesAddResult.Succeeded ? null : rolesAddResult.Errors.Select(e => new Error(e.Code, e.Description))); ;
        }

        public async Task<string[]> GetUserRoles(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.GetRolesAsync(appUser);
            return result.ToList().ToArray();
        }

        public async Task<LDAPIdentityResult> CheckPasswordLdap(string userName, string password)
        {
            var result = await _ldapUserManager.CheckPasswordAsync(userName, password);
            return result;
        }
    }
}
