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
    public class UsersModel : PageModel
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public List<BlogUser> Users { get; set; }

        public Dictionary<string, IList<string>> UserRoles { get; set; }
        #region User informations

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public DateTime? UserBirthDate { get; set; }
        private void SetUserData(BlogUser user)
        {
            UserName = user.Name;
            UserSurname = user.Surname;
            UserBirthDate = user.BirthDate;
        }

        #endregion

        public UsersModel(UserManager<BlogUser> userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
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
            var admin = await _userManager.GetUserAsync(HttpContext.User);

            SetUserData(admin);

            Users = _userManager.Users.ToList();

            await GetRoles();

            return Page();
        }
    }
}
