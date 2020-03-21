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
    [Authorize(Roles = "Administrator, Za³o¿yciel")]
    public class UsersModel : PageModel
    {
        #region Private variables

        private readonly UserManager<BlogUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        #endregion

        #region Public variables
        public Dictionary<string, IList<string>> UserRoles { get; set; }
        public List<BlogUser> Users { get; set; }
        public bool isAdministrator { get; set; }
        public BlogUser LoggedUser { get; set; }

        #endregion

        public UsersModel(UserManager<BlogUser> userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
            isAdministrator = false;
            LoggedUser = new BlogUser();
        }

        private async Task GetRoles()
        {
            UserRoles = new Dictionary<string, IList<string>>();

            foreach(var User in Users)
            {
                var userRole = await _userManager.GetRolesAsync(User);

                UserRoles.Add(User.Id,userRole);
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            isAdministrator = await _userManager.IsInRoleAsync(user, ("Administrator")) || await _userManager.IsInRoleAsync(user, "Za³o¿yciel");

            LoggedUser = user;

            Users = _userManager.Users.ToList();

            await GetRoles();

            return Page();
        }
    }
}
