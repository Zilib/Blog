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
        public string EditedUserNick { get; set; }
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

        // User data informations
        [BindProperty]
        public InputModel Input { get; set; }

        #endregion

        #region Input Model

        public class InputModel
        {
            #region Required

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [Display(Name ="Nazwa u¿ytkownika")]
            public string NewUserName { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string NewEmail { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [Display(Name = "Imiê")]
            public string NewName { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [Display(Name = "Nazwisko")]
            public string NewSurname { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane!")]
            [Display(Name = "Data urodzenia")]
            [DataType(DataType.Date, ErrorMessage = "Wartoœæ jest nieprawid³owa")]
            public DateTime? NewBirthDate { get; set; }

            #endregion

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Nr. Telefonu")]
            public string NewPhoneNumber { get; set; }
        }

        #endregion

        #region Private functions

        private void SetInputValues(BlogUser user)
        {
            if (Input == null)
                Input = new InputModel();

            Input.NewName = user.Name;
            Input.NewSurname = user.Surname;
            Input.NewUserName = user.UserName;
            Input.NewEmail = user.Email;
            Input.NewPhoneNumber = user.PhoneNumber;
            Input.NewBirthDate = user.BirthDate;
        }

        private bool UpdateEditedUser(BlogUser editedUser)
        {
            // If something went wrong, and input doesn't exist
            if (Input == null)
                return false;

            editedUser.Name = Input.NewName;
            editedUser.Surname = Input.NewSurname;
            editedUser.UserName = Input.NewUserName;
            editedUser.Email = Input.NewEmail;
            editedUser.PhoneNumber = Input.NewPhoneNumber;
            editedUser.BirthDate = Input.NewBirthDate;

            return true;
        }

        /// <summary>
        /// Set roles variable's, setted and unsetted
        /// </summary>
        private async Task GetRoles(BlogUser EditedUser)
        {
            EditedUserRoles = new List<string>();
            EditedUserRoles = await _userManager.GetRolesAsync(EditedUser);
            var AllAvailableRoles = _roleManager.Roles.ToList();

            NoEditedUserRoles = new List<string>();

            // Find roles which are not assigned to edited user
            foreach (var role in AllAvailableRoles.ToList())
            {
                if (!EditedUserRoles.Contains(role.ToString()))
                NoEditedUserRoles.Add(role.ToString());
            }
        }

        #endregion

        #region OnGet

        public async Task<IActionResult> OnGetAsync(string userId = null)
        {
            #region Validate logged user(admin)

            LoggedUser = await _userManager.GetUserAsync(HttpContext.User);

            if (!await _userManager.IsInRoleAsync(LoggedUser, "Administrator"))
                return RedirectToPage("/Account", new { area = "Admin" });

            isAdministrator = true; // if user is admin, tell it to the app

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

            #region Validate edited user and set input model

            var EditedUser = await _userManager.FindByIdAsync(userId);
            EditedUserNick = EditedUser.UserName;

            // User not found
            if (EditedUser == null)
            {
                _logger.LogInformation("User not found");
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            // Show current values
            SetInputValues(EditedUser);

            // Set it for form
            EditedUserId = userId;

            #endregion

            await GetRoles(EditedUser);

            return Page();
        }

        #endregion

        #region OnPost

        public async Task<IActionResult> OnPostAsync()
        {
            #region Validate logged user(admin)

            LoggedUser = await _userManager.GetUserAsync(HttpContext.User);

            if (!await _userManager.IsInRoleAsync(LoggedUser, "Administrator"))
                return RedirectToPage("/Account", new { area = "Admin" });

            isAdministrator = true; // if user is admin, tell it to the app

            #endregion

            #region Validate edited user

            if (EditedUserId == null)
            {
                _logger.LogInformation("I couldn't modify user");
                return RedirectToPage("/Users", new { area = "Admin" });
            }

            // Post checking
            var EditedUser = await _userManager.FindByIdAsync(EditedUserId);
            EditedUserNick = EditedUser.Name;

            if (EditedUser == null)
            {
                _logger.LogInformation("User not found HERE");
                return RedirectToPage("/Users", new { area = "Admin" });
            }

            #endregion

            #region Validate form

            if (!ModelState.IsValid)
            {
                LoggedUser = await _userManager.GetUserAsync(HttpContext.User);
                await GetRoles(EditedUser);

                return Page();
            }

            #endregion

            #region Update user

            // Set updated user data
            if (!UpdateEditedUser(EditedUser))
            {
                _logger.LogInformation("Input is null");
                return RedirectToPage("Users", new { area = "Admin" });
            }

            // If this email is in use, forbidden to edit his acc
            if (await _userManager.FindByEmailAsync(Input.NewEmail) != null)
            {
                ModelState.AddModelError(string.Empty, "Ten adres email jest u¿ywany");
                await GetRoles(EditedUser);

                return Page();
            }

            IdentityResult result = await _userManager.UpdateAsync(EditedUser);
            if (result.Succeeded)
            {
                _logger.LogInformation("User modifed successful");
            }
            else
            {
                _logger.LogInformation("Sorry i couldn't modify user :(");

                await GetRoles(EditedUser);

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            #endregion

            return RedirectToPage("/Users", new { area = "Admin" });
        }

        #endregion
    }
}
