using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Identity.Data;
using Blog.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Blog.Areas.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public string userName { get; set; }
        public string userSurname { get; set; }

        [BindProperty]
        public UserInformation userInformation { get; set; }

        public DashboardModel(UserManager<BlogUser> userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public class UserInformation
        {
            [Required]
            [Display(Name = "Imie")]
            public string Name { get; set; }
            [Required]
            [Display(Name = "Nazwisko")]
            public string Surname { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            userInformation = new UserInformation
            {
                Name = user.Name,
                Surname = user.Surname
            };
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (userInformation.Name != user.Name && userInformation.Name != "")
            {
                user.Name = userInformation.Name;
            }

            if (userInformation.Surname != user.Surname && userInformation.Surname != "")
            {
                user.Surname = userInformation.Surname;
            }

            await _userManager.UpdateAsync(user);
            return Page();
        }
    }
}
