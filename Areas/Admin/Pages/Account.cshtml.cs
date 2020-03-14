using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Blog.Areas.Admin
{
    public class AccountModel : PageModel
    {
        #region Private variables

        private readonly UserManager<BlogUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        #endregion
        #region Construct

        public AccountModel(UserManager<BlogUser> userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        #endregion

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

        #region Input Data Model

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Imie")]
            public string NewName { get; set; }

            [Required]
            [Display(Name = "Nazwisko")]
            public string NewSurname { get; set; }

            [Required]
            [Display(Name = "Data Urodzenia")]
            [DataType(DataType.Date)]
            public DateTime NewBirthDate { get; set; }
        }

        #endregion

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            SetUserData(user);

            Input = new InputModel
            {
                NewName = user.Name,
                NewSurname = user.Surname,
                NewBirthDate = user.BirthDate
            };
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            if (!ModelState.IsValid)
            {
                SetUserData(user);

                return Page();
            }

            #region Input Validation

            if (Input.NewName != user.Name)
            {
                user.Name = Input.NewName;
            }

            if (Input.NewSurname != user.Surname)
            {
                user.Surname = Input.NewSurname;
            }

            if (Input.NewBirthDate != user.BirthDate)
            {
                user.BirthDate = Input.NewBirthDate;
            }

            #endregion

            await _userManager.UpdateAsync(user);

            // Local redirect because, i want to reload data.
            return RedirectToPage("/Account", new { area = "Admin" });
        }
    }
}
