using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Blog.Areas.Admin.Pages
{
    [Authorize(Roles = "Administrator")]
    public class UserRemoveRoleModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<BlogUser> _userManager;
        public UserRemoveRoleModel(ILogger<LoginModel> logger, UserManager<BlogUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return RedirectToPage("/Account", new { area = "Admin" });
        }
        public async  Task<IActionResult> OnPost()
        {
            #region Logged user assign to variables

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(!await _userManager.IsInRoleAsync(user, "Administrator")
                && !await _userManager.IsInRoleAsync(user, "Za³o¿yciel"))
            {
                _logger.LogInformation("You are not able to remove role!");
                return RedirectToPage("/Account", new { area = "Admin" });
            }

            #endregion

            #region Form validation

            var roleToRemove = Request.Form["UserRole"].ToString();

            // Depend of page, if we are working with edit page name of input with user id is EditedUserId, but if we are working with account page. Name of input where is user id is UserId 
            var UserId = Request.Form["UserId"].ToString() == string.Empty ? Request.Form["EditedUserId"].ToString() : Request.Form["UserId"].ToString();

            if (string.IsNullOrEmpty(roleToRemove) || string.IsNullOrEmpty(UserId))
            {
                _logger.LogInformation("Something is wrong");
                return RedirectToPage("/Account", new { area = "Admin" });
            }

            #endregion

            #region Remove role
            
            // Find user who will lost his privilages
            var UserToRemoveRole = await _userManager.FindByIdAsync(UserId);
            if (UserToRemoveRole == null)
            {
                _logger.LogInformation("No user id");
                return RedirectToPage("/Account", new { area = "Admin" });
            }

            await _userManager.RemoveFromRoleAsync(UserToRemoveRole, roleToRemove);

            #endregion

            // Back to last page. If you edited your own profile you go back to account page, if not it is probabbly EditUser
            if (Request.Headers["Referer"].ToString() != string.Empty)
                return Redirect(Request.Headers["Referer"].ToString());

            return RedirectToPage("/Account", new { area = "Admin" });
        }
    }
}
