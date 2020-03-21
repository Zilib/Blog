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
            // Depend of page, if we are working with edit page name of input with user id is EditedUserId, but if we are working with account page. Name of input where is user id is UserId 
            var UserId = Request.Form["UserId"].ToString() == string.Empty ? Request.Form["EditedUserId"].ToString() : Request.Form["UserId"].ToString();

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

                // Back to last page. If you edited your own profile you go back to account page, if not it is probabbly EditUser
                if (Request.Headers["Referer"].ToString() != string.Empty)
                    return Redirect(Request.Headers["Referer"].ToString());
            } 
            else
            {
                _logger.LogInformation("Oups, something went wrong. I couldn't assign role to the user");
            }

            return RedirectToPage("/Account", new { area = "Admin" });
        }
    }
}
