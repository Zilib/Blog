using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<BlogUser> _userManager;

        public AccountController(UserManager<BlogUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> DashboardAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return View(user);
        }
    }
}