using Microsoft.AspNetCore.Identity;
using PermissionBasedAuthorizationInDotNet5.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PermissionBasedAuthorizationInDotNet5.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager)
        {
            var defaultUser = new IdentityUser
            {
                UserName = "basicuser@gmail.com",
                Email = "basicuser@gmail.com",
                EmailConfirmed =true 
            };
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user ==null )
            {
                await userManager.CreateAsync(defaultUser, "P@ssword123");
                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
            }
        }

        public static async Task SeedSuperAdminUserAsync(UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new IdentityUser
            {
                UserName = "SuperAdminUser@gmail.com",
                Email = "SuperAdminUser@gmail.com",
                EmailConfirmed = true
            };
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssword123");
                await userManager.AddToRolesAsync(defaultUser,new List<string> { Roles.Basic.ToString() , Roles.Admin.ToString() , Roles.SuperAdmin.ToString() });
            }
            //TODO SeedClaims
            await roleManager.SeedClaiamsForSuperUser();
        }

        private static async Task SeedClaiamsForSuperUser(this RoleManager<IdentityRole> roleManager)
        {
            var superRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            await roleManager.AddPermissionClaims(superRole, "Products");
        }

        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager , IdentityRole role , string model)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsList(model);

            foreach (var Permission in allPermissions)
            {
                if (!allClaims.Any(c=> c.Type == "Permission" && c.Value == Permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", Permission));
                }
            }
        }
    }
}
