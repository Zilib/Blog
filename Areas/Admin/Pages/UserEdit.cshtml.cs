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
    public class UserEditModel : PageModel
    {
        #region Construct

        public UserEditModel(UserManager<BlogUser> userManager,
            ILogger<LoginModel> logger,
            RoleManager<IdentityRole> roleManager)
        {
            #region Identity inject

            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;

            #endregion

            #region This class variables

            isAdministrator = false;

            #endregion

            #region This class's collections and objects

            LoggedUser = new BlogUser();
            EditedUser = new BlogUser();
            EditedUserRoles = new List<string>();
            NoEditedUserRoles = new List<string>();

            #endregion
        }

        #endregion

        #region Private variables

        private readonly UserManager<BlogUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        #endregion

        #region Public variables

        // In assumptions it is administrator
        public BlogUser LoggedUser { get; set; }
        public bool isAdministrator { get; set; }
        // Roles which are assigned to edited user
        public IList<string> EditedUserRoles { get; set; }
        // Roles which are not assigned to edited user user
        public List<string> NoEditedUserRoles { get; set; }

        #endregion

        #region Getters

        /// <summary>
        /// Receive number of edited user roles
        /// </summary>
        /// <returns></returns>
        public int NumberOfRoles() => EditedUserRoles.Count();

        /// <summary>
        /// Receive number of roles which are not assigned to the user!
        /// </summary>
        public int NumberOfNoRoles() => NoEditedUserRoles.Count();
        #endregion

        #region Binds

        [BindProperty]
        [HiddenInput]
        public string EditedUserId { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        /// <summary>
        /// Model of user who actually is modyfing
        /// </summary>
        [BindProperty]
        public BlogUser EditedUser { get; set; }

        #endregion

        #region Input Model

        public class InputModel
        {
            [Required]
            [Display(Name = "Imiê")]
            public string NewName { get; set; }

            [Required]
            [Display(Name = "Nazwisko")]
            public string NewSurname { get; set; }

            [Required]
            [Display(Name = "Data urodzenia")]
            [DataType(DataType.Date)]
            public DateTime? NewBirthDate { get; set; }
        }

        #endregion

        #region OnGet

        public async Task<IActionResult> OnGetAsync(string userId = null)
        {
            #region Validate user, and assign him

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!await _userManager.IsInRoleAsync(user, "Administrator"))
                return RedirectToPage("/Account", new { area = "Admin" });

            isAdministrator = true; // if user is admin, tell it to the app
            LoggedUser = user; // It is administrator

            #endregion

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation("Id is not passed");
                return RedirectToPage("/Account", new { area = "Admin" });
            }

            // If admin is trying to edit himself redirect him to account page
            if (LoggedUser.Id == userId)
            {
                _logger.LogInformation("Admin is trying to edit his own acc");
                return RedirectToPage("/Account", new { area = "Admin" });
            }

            #region Set variables value

            EditedUser = await _userManager.FindByIdAsync(userId);

            // User not found
            if (EditedUser == null)
            {
                _logger.LogInformation("User not found");
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            // Show current values
            Input = new InputModel();
            Input.NewName = EditedUser.Name;
            Input.NewSurname = EditedUser.Surname;
            Input.NewBirthDate = EditedUser.BirthDate;

            // Set it for form
            EditedUserId = userId;

            #endregion

            #region Roles

            EditedUserRoles = await _userManager.GetRolesAsync(EditedUser);
            var AllAvailableRoles = _roleManager.Roles.ToList();

            // Find roles which are not assigned to edited user
            foreach (var role in AllAvailableRoles.ToList())
            {
                if (!EditedUserRoles.Contains(role.ToString()))
                    NoEditedUserRoles.Add(role.ToString());
            }
            _logger.LogInformation(EditedUserRoles.Count().ToString());

            #endregion

            return Page();
        }

        #endregion

        #region OnPost

        public async Task<IActionResult> OnPostAsync()
        {
            if (EditedUserId == null)
            {
                _logger.LogInformation("I couldn't modify user");
                return RedirectToPage("/Users", new { area = "Admin" });
            }

            // Post checking
            EditedUser = await _userManager.FindByIdAsync(EditedUserId);

            if (EditedUser == null)
            {
                _logger.LogInformation("User not found HERE");
                return RedirectToPage("/Users", new { area = "Admin" });
            }

            // Set updated user data
            EditedUser.Name = Input.NewName;
            EditedUser.Surname = Input.NewSurname;
            EditedUser.BirthDate = Input.NewBirthDate;

            IdentityResult result = await _userManager.UpdateAsync(EditedUser);
            if (result.Succeeded)
            {
                _logger.LogInformation("User modifed successful");
            }
            else
            {
                _logger.LogInformation("Sorry i couldn't modify user :(");
            }

            return RedirectToPage("/Users", new { area = "Admin" });
        }

        #endregion
    }
}
