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
    public class UserDeleteModel : PageModel
    {
        private readonly UserManager<BlogUser> _userManager;

        public string Message { get; set; }

        #region User informations

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public DateTime? UserBirthDate { get; set; }

        /// <summary>
        /// Load data for sidebar
        /// </summary>
        /// <param name="user"></param>
        private void SetUserData(BlogUser user)
        {
            UserName = user.Name;
            UserSurname = user.Surname;
            UserBirthDate = user.BirthDate;
        }

        #endregion

        public UserDeleteModel(UserManager<BlogUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            var admin = await _userManager.GetUserAsync(HttpContext.User);

            SetUserData(admin);

            if (admin.Id == userId)
            {
                Message = "Nie mo�esz skasowa� swojego konta.";
                return Page();
            }

            var userToRemove = await _userManager.FindByIdAsync(userId);

            if (userToRemove == null)
            {
                Message = "Nie znaleziono u�ytkownika z takim Id.";
                return Page();
            }

            IdentityResult result = await _userManager.DeleteAsync(userToRemove);
            if (result.Succeeded)
                Message = "Usuni�to u�ytkownika.";
            else
                Message = "Co� posz�o nie tak, nie uda�o si� skasowa� u�ytkownika.";

            return Page();
        }
    }
}
