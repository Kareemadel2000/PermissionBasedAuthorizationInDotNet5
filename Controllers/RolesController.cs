using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermissionBasedAuthorizationInDotNet5.Constant;
using PermissionBasedAuthorizationInDotNet5.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionBasedAuthorizationInDotNet5.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", await _roleManager.Roles.ToListAsync());

            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "Role is exists!");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }

            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            var roles = await _roleManager.FindByIdAsync(roleId);
            if (roles == null)
                return NotFound();

            var roleCalims = _roleManager.GetClaimsAsync(roles).Result.Select(c=>c.Value).ToList();
            var allPermission = Permissions.GenerateAllPermissions().Select(p=> new CheckBoxViewModel { DisplayValue=p}).ToList();

            foreach (var premission in allPermission)
            {
                if (roleCalims.Any(c => c == premission.DisplayValue))
                    premission.IsSelected = true;
            }

            var viewModel = new PermissionsFormViewModel 
            { 
                RoleId=roleId,
                RoleName = roles.Name,
                RoleCalims = allPermission
            };
            return View(viewModel);
        }
    }
}
