using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blog.Areas.Admin.Pages
{
    public class UserEditModel : PageModel
    {
        private readonly UserManager<BlogUser> _userManager;

        public string Action { get; set; }

        // User who's administrator edit right now
        public BlogUser CurrentEditUser { get; set; }

        public UserEditModel(UserManager<BlogUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult OnGet(BlogUser User)
        {
            Action = "Get";
            return Page();
        }

        public IActionResult OnPost(BlogUser User)
        {
            Action = Request.Form["UserId"];
            
            return Page();
        }
    }
}
