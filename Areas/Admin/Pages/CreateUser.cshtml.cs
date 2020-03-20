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
        public bool isAdministrator { get; set; }

        public BlogUser LoggedUser { get; set; }

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
            isAdministrator = false;
            LoggedUser = new BlogUser();
        }

        #endregion


        public class InputModel
        {
            #region Required
            
            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [Display(Name = "Nazwa u¿ytkownika")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [DataType(DataType.Password)]
            [Display(Name = "Has³o")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [Display(Name = "PotwierdŸ has³o")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Has³a do siebie nie pasuj¹")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [Display(Name = "Imiê")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [Display(Name = "Nazwisko")]
            public string Surname { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [Display(Name = "Data urodzenia")]
            [DataType(DataType.Date, ErrorMessage = "Wartoœæ jest nieprawid³owa")]
            public DateTime? BirthDate { get; set; }

            #endregion

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Nr. Telefonu")]
            public string MobilePhone { get; set; }
            
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // Only administrator has access to this page
            if (!await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                return RedirectToPage("/Account", new { area = "Admin" });
            }
            // only admin can be at this site, so he must be a admin!
            isAdministrator = true;

            LoggedUser = user;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // Only administrator has access to this page
            if (!await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                return RedirectToPage("/Account", new { area = "Admin" });
            }

            isAdministrator = true;
            LoggedUser = user;

            if (ModelState.IsValid)
            {
                //Check does any account is assgined to this adress email
                bool emailInUse = await _userManager.FindByEmailAsync(Input.Email) == null ? false : true;

                if (emailInUse)
                {
                    ModelState.AddModelError(string.Empty, "Dany adres email jest ju¿ u¿ywany!");
                    return Page();
                }

                // Create new user which will be inserted into table
                var newUser = new BlogUser
                {
                    UserName = Input.UserName,
                    Email = Input.Email,
                    Name = Input.Name,
                    Surname = Input.Surname,
                    BirthDate = Input.BirthDate,
                    PhoneNumber = Input.MobilePhone,
                    EmailConfirmed = true
                };

                // Register user
                var result = await _userManager.CreateAsync(newUser, Input.Password);
                 
                // if everything is fine
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "U¿ytkownik");
                    // Show information, user is added correctly
                    ModelState.AddModelError(string.Empty, "Prawid³owo dodano u¿ytkownika");
                    return Page();
                }

                // Add errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            return Page();
        }
    }
}
