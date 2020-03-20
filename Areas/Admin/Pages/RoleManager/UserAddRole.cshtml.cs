using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Blog.Areas.Admin.Pages.RoleManager
{
    public class UserAddRoleModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<BlogUser> _userManager;
        public UserAddRoleModel(
            ILogger<LoginModel> logger,
            UserManager<BlogUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        public IActionResult OnGet()
        {
            return RedirectToPage("/Account", new { area = "Admin" });
        }

        public async Task<IActionResult> OnPost()
        {
            var UserId = Request.Form["UserId"].ToString();
            var RoleToAssign = Request.Form["RoleToAssign"].ToString();

            if (UserId == string.Empty || RoleToAssign == string.Empty)
            {
                _logger.LogInformation("Something is wrong");
                return RedirectToPage("/Account", new { area = "Admin" });
            }

            var UserToAssignRole = await _userManager.FindByIdAsync(UserId);
            if (UserToAssignRole == null)
            {
                _logger.LogInformation("User not found");
                return RedirectToPage("/Account", new { area = "Admin" });
            }

            IdentityResult result = await _userManager.AddToRoleAsync(UserToAssignRole, RoleToAssign);
            if (result.Succeeded)
            {
                _logger.LogInformation("Role's assigned successful to the user");
            } 
            else
            {
                _logger.LogInformation("Oups, something went wrong. I couldn't assign role to the user");
            }

            return RedirectToPage("/Account", new { area = "Admin" });
        }
    }
}
