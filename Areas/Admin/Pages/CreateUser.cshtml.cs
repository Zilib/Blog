using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Data;
using Microsoft.AspNetCore.Authentication;
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
        private readonly SignInManager<BlogUser> _signInManager;

        #endregion

        #region Public variables 
        
        [BindProperty]
        public InputModel Input { get; set; }

        #endregion

        #region Construct
        public CreateUserModel(
            ILogger<LoginModel> logger,
            UserManager<BlogUser> userManager,
            SignInManager<BlogUser> signInManager
            )
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
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
            [Required(ErrorMessage = "Pole '{0}' jest wymagane!")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Pole '{0}' jest wymagane!")]
            [DataType(DataType.Password)]
            [Display(Name = "Has³o")]
            public string Password { get; set; }

            [Required]
            [Display(Name = "PotwierdŸ has³o")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Has³a do siebie nie pasuj¹")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Pole '{0}' jest wymagane!")]
            [Display(Name = "Imiê")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Pole '{0}' jest wymagane!")]
            [Display(Name = "Nazwisko")]
            public string Surname { get; set; }

            [Required(ErrorMessage = "Pole '{0}' jest wymagane!")]
            [Display(Name = "Data urodzenia")]
            [DataType(DataType.Date, ErrorMessage = "Wartoœæ jest nieprawid³owa")]
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

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            if (ModelState.IsValid)
            {
                var newUser = new BlogUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    Surname = Input.Surname,
                    BirthDate = Input.BirthDate,
                    PhoneNumber = Input.MobilePhone
                };

                var result = await _userManager.CreateAsync(newUser, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Wszystko okej, uzytkownik zarejestrowany");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = newUser.Id, code = code},
                        protocol: Request.Scheme);

                    
                }
                return Page();
            }

            return Page();
        }
    }
}
