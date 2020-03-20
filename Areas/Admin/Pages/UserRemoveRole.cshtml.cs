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
            var roleToRemove = Request.Form["UserRole"].ToString();
            var userId = Request.Form["UserId"].ToString();

            if (string.IsNullOrEmpty(roleToRemove) || string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account", new { area = "Admin" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogInformation("No user id");
                return RedirectToPage("/Account", new { area = "Admin" });
            }

            await _userManager.RemoveFromRoleAsync(user, roleToRemove);

            return RedirectToPage("/Account", new { area = "Admin" });
        }
    }
}
