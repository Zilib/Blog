using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blog.Areas.Admin.Pages
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<BlogUser> _userManager;

        public List<BlogUser> Users;

        [BindProperty]
        public string UserId { get; set; }

        #region User informations

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public DateTime UserBirthDate { get; set; }
        private void SetUserData(BlogUser user)
        {
            UserName = user.Name;
            UserSurname = user.Surname;
            UserBirthDate = user.BirthDate;
        }

        #endregion
        public UsersModel(UserManager<BlogUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            SetUserData(user);

            Users = _userManager.Users.ToList();

            return Page();
        }
    }
}
