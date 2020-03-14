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

namespace Blog.Areas.Admin.Pages
{
    public class CreateUserModel : PageModel
    {
        #region Private variables

        private readonly UserManager<BlogUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        #endregion

        #region Public variables 
        
        public InputModel Input { get; set; }

        #endregion

        #region Construct
        public CreateUserModel(
            ILogger<LoginModel> logger,
            UserManager<BlogUser> userManager
            )
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

        public class InputModel
        {
            #region Required

            [Required]
            [Display(Name = "Nazwa u¿ytkownika")]
            public string NickName { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Has³o")]
            public string Password { get; set; }

            [Required]
            [Display(Name = "Imiê")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Nazwisko")]
            public string Surname { get; set; }

            [Required]
            [Display(Name = "Data urodzenia")]
            [DataType(DataType.Date)]
            public DateTime BirthDate { get; set; }

            #endregion

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Nr. Telefonu")]
            public string MobilePhone { get; set; }
            
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            SetUserData(user);

            return Page();
        }
    }
}
