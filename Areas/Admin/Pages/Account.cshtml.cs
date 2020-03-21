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
        private readonly string[] ForbiddenRolesToRemove = { "Administrator", "Za這篡ciel" };

        #endregion

        #region Public variables
        public bool isAdministrator { get; set; }
        public BlogUser LoggedUser { get; set; }
        public IList<string> UserRoles { get; set; }
        // List of roles where user is not assigned
        public List<string> RolesAvailableToAdd { get; set; }

        #endregion

        #region Construct

        public AccountModel(UserManager<BlogUser> userManager,
            ILogger<LoginModel> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            isAdministrator = false;
            LoggedUser = new BlogUser();
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

        #region Private functions
        /// <summary>
        /// Set all role variables, forbbid removing administrator.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task GetRoles(BlogUser user, BlogUser Admin)
        {
            var AllRoles = _roleManager.Roles.ToList();

            // If user is the administrator, don't show it him. He know about it, disallow him to remove the most powerful function
            UserRoles = await _userManager.GetRolesAsync(user);

            // Za這篡ciel can give for another user administrator, so give him this privilage
            RolesAvailableToAdd = new List<string>();
            if (await _userManager.IsInRoleAsync(Admin, "Za這篡ciel") && !await _userManager.IsInRoleAsync(Admin,"Administrator"))
                RolesAvailableToAdd.Add("Administrator");

            //Only Za這篡ciel can add and remove administrator role
            if (!UserRoles.Contains("Za這篡ciel") && UserRoles.Contains("Administrator"))
                UserRoles.Remove("Administrator");
            if (UserRoles.Contains("Za這篡ciel"))
                UserRoles.Remove("Za這篡ciel");


            // Fill the array, of the values which are not assigned to the user
            foreach (var role in AllRoles)
            {
                if (!ForbiddenRolesToRemove.Contains(role.ToString())
                    && !UserRoles.Contains(role.ToString()))
                    RolesAvailableToAdd.Add(role.ToString());
            }
        }

        /// <summary>
        /// When user is a admin
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task GetRoles(BlogUser user)
        {
            var AllRoles = _roleManager.Roles.ToList();

            // If user is the administrator, don't show it him. He know about it, disallow him to remove the most powerful function
            UserRoles = await _userManager.GetRolesAsync(user);

            // Za這篡ciel can give for another user administrator, so give him this privilage
            RolesAvailableToAdd = new List<string>();
            if (!UserRoles.Contains("Administrator") 
                && await _userManager.IsInRoleAsync(user, "Za這篡ciel")
                && await _userManager.IsInRoleAsync(user, "Administrator"))
                {
                    RolesAvailableToAdd.Add("Administrator");
                }

            //Only Za這篡ciel can add and remove administrator role
            if (!UserRoles.Contains("Za這篡ciel") && UserRoles.Contains("Administrator"))
                UserRoles.Remove("Administrator");
            if (UserRoles.Contains("Za這篡ciel"))
                UserRoles.Remove("Za這篡ciel");


            // Fill the array, of the values which are not assigned to the user
            foreach (var role in AllRoles)
            {
                if (!ForbiddenRolesToRemove.Contains(role.ToString())
                    && !UserRoles.Contains(role.ToString()))
                    RolesAvailableToAdd.Add(role.ToString());
            }
        }
        #endregion

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            isAdministrator = await _userManager.IsInRoleAsync(user, "Administrator") || await _userManager.IsInRoleAsync(user,"Za這篡ciel");

            LoggedUser = user;
            UserId = LoggedUser.Id;

            Input = new InputModel
            {
                NewName = LoggedUser.Name,
                NewSurname = LoggedUser.Surname,
                NewBirthDate = LoggedUser.BirthDate
            };

            await GetRoles(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            isAdministrator = await _userManager.IsInRoleAsync(user, ("Administrator"));

            if (!ModelState.IsValid)
            {
                LoggedUser = user;

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

            // Local redirect because, i want to reload data. And go to the get method
            return RedirectToPage("/Account", new { area = "Admin" });
        }
    }
}
