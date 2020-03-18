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

        #region Logged user informations

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
            public DateTime NewBirthDate { get; set; }
        }

        #endregion

        #region Edited user infomations

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string editedUserId { get; set; }

        #endregion

        public UserEditModel(UserManager<BlogUser> userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string userId = null)
        {
            BlogUser admin = await _userManager.GetUserAsync(HttpContext.User);

            if (admin == null)
            {
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation("Id is not passed");
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            // If admin is trying to edit himself redirect him to account page
            if (admin.Id == userId)
            {
                _logger.LogInformation("Admin is trying to edit his own acc");
                return RedirectToPage("/Account", new { area = "Admin" });
            }
            
            var editedUser = await _userManager.FindByIdAsync(userId);


            if (editedUser == null)
            {
                _logger.LogInformation("User not found");
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            // Show current values
            Input = new InputModel();
            Input.NewName = editedUser.Name;
            Input.NewSurname = editedUser.Surname;
            Input.NewBirthDate = editedUser.BirthDate;

            // Can be modified
            editedUserId = userId;
          
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var admin = await _userManager.GetUserAsync(HttpContext.User);

            if (admin == null)
            {
                return RedirectToPage("/Login", new { area = "Admin" });
            }

            if (editedUserId == null)
            {
                return RedirectToPage("/Users", new { area = "Admin" });
            }

            // Post checking
            var userToEdit = await _userManager.FindByIdAsync(editedUserId);

            if (userToEdit == null)
            {
                _logger.LogInformation("User not found HERE");
                return RedirectToPage("/Users", new { area = "Admin" });
            }

            // Set updated user data
            userToEdit.Name = Input.NewName;
            userToEdit.Surname = Input.NewSurname;
            userToEdit.BirthDate = Input.NewBirthDate;

            await _userManager.UpdateAsync(userToEdit);

            return RedirectToPage("/Users", new { area = "Admin" });
        }
    }
}
