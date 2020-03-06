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

        public string UserName { get; set; }
        public string UserSurname { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public DashboardModel(UserManager<BlogUser> userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Imie")]
            public string NewName { get; set; }
            [Required]
            [Display(Name = "Nazwisko")]
            public string NewSurname { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return LocalRedirect("~/Admin/Login");
            }

            UserName = user.Name;
            UserSurname = user.Surname;

            Input = new InputModel
            {
                NewName = user.Name,
                NewSurname = user.Surname
            };
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return LocalRedirect("~/Admin/Login");
            }

            if (!ModelState.IsValid)
            {
                UserName = user.Name;
                UserSurname = user.Surname;

                return Page();
            }

            #region Input Validation

            if (Input.NewName != user.Name && Input.NewName != string.Empty)
            {
                user.Name = Input.NewName;
            }

            if (Input.NewSurname != user.Surname && Input.NewSurname != string.Empty)
            {
                user.Surname = Input.NewSurname;
            }

            #endregion

            await _userManager.UpdateAsync(user);

            // Local redirect because, i want to reload data.
            return LocalRedirect("~/Admin/Dashboard");
        }
    }
}
