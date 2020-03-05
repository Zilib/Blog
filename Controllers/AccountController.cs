using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Identity.Data;
using Blog.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public InputModel Input { get; set; }

        public string UserName { get; set; }
        public AccountController(UserManager<BlogUser> userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public class InputModel
        {
            [Required]
            [Display(Name= "Imie")]
            public string Name { get; set; }
            [Required]
            [Display(Name = "Nazwisko")]
            public string Surname { get; set; }
        }

        private async Task LoadAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            UserName = user.Name;
            Input = new InputModel
            {
                Name = user.Name
            };
        }

        [Authorize]
        public async Task<IActionResult> DashboardAsync()
        {
            await LoadAsync();

            return View();
        }

        [Authorize]
        public  IActionResult UpdateDataAsync()
        {
            return View(Input);
        }
    }
}