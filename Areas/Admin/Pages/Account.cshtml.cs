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
        private readonly RoleManager<IdentityRole> _roleManager;

        #endregion

        #region Construct

        public AccountModel(UserManager<BlogUser> userManager,
            ILogger<LoginModel> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        #endregion

        #region Logged user informations

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public DateTime? UserBirthDate { get; set; }
        public IList<string> UserRoles { get; set; }

        private async Task SetUserData(BlogUser user)
        {
            UserName = user.Name;
            UserSurname = user.Surname;
            UserBirthDate = user.BirthDate;
            UserId = user.Id;

            AbleToModifyRoles = await _userManager.IsInRoleAsync(user, "Administrator");
            UserRoles = await _userManager.GetRolesAsync(user);
        }

        #endregion

        #region Input Data Model

        [BindProperty]
        [HiddenInput]
        [Display(Name = "UserId")]
        public string UserId { get; set; }

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
            public DateTime? NewBirthDate { get; set; }

        }

        #endregion

        public List<IdentityRole> AllRoles { get; set; }   
        // List of roles where user is not assigned
        public List<string> NoUserRoles { get; set; }
        public bool AbleToModifyRoles { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var admin = await _userManager.GetUserAsync(HttpContext.User);

            await SetUserData(admin);

            Input = new InputModel
            {
                NewName = admin.Name,
                NewSurname = admin.Surname,
                NewBirthDate = admin.BirthDate
            };

            AllRoles = _roleManager.Roles.ToList();

            // If user is the administrator, don't show it him. He know about it, disallow him to remove the most powerful function
            if (UserRoles.Contains("Administrator"))
                UserRoles.Remove("Administrator");

            // Fill the array, of the values which are not assigned to the user
            NoUserRoles = new List<string>();
            foreach (var role in AllRoles)
            {
                if (role.ToString() != "Administrator" 
                    && !UserRoles.Contains(role.ToString()))
                    NoUserRoles.Add(role.ToString());
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var admin = await _userManager.GetUserAsync(HttpContext.User);

            if (!ModelState.IsValid)
            {
                await SetUserData(admin);

                return Page();
            }

            #region Input Validation

            if (Input.NewName != admin.Name)
            {
                admin.Name = Input.NewName;
            }

            if (Input.NewSurname != admin.Surname)
            {
                admin.Surname = Input.NewSurname;
            }

            if (Input.NewBirthDate != admin.BirthDate)
            {
                admin.BirthDate = Input.NewBirthDate;
            }

            #endregion

            await _userManager.UpdateAsync(admin);

            // Local redirect because, i want to reload data. And go to the get method
            return RedirectToPage("/Account", new { area = "Admin" });
        }
    }
}
