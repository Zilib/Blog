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
        private readonly UserManager<BlogUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        [BindProperty]
        public string EditedUserId { get; set; }

        public BlogUser LoggedUser { get; set; }
        [BindProperty]
        public BlogUser EditedUser { get; set; }
        public bool isAdministrator { get; set; }


        #region Construct

        public UserEditModel(UserManager<BlogUser> userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
            isAdministrator = false;
            LoggedUser = new BlogUser();
            EditedUser = new BlogUser();
        }

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

        #region Edited user infomations

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string editedUserId { get; set; }

        #endregion


        public async Task<IActionResult> OnGetAsync(string userId = null)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!await _userManager.IsInRoleAsync(user, "Administrator"))
                return RedirectToPage("/Account", new { area = "Admin" });

            isAdministrator = true;
            LoggedUser = user;

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
            
            EditedUser = await _userManager.FindByIdAsync(userId);

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

            // Can be modified
            EditedUserId = userId;

            return Page();
        }

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
    }
}
