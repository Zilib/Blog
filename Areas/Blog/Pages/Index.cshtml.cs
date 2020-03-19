using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blog.Areas.Data;

namespace Blog.Areas.Blog
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly ApplicationContext _applicationContext;
        public List<Post> Posts { get; set; }

        // if posts are empty show information "No posts to show"
        public bool showPosts() => Posts.Count() > 0;

        public IndexModel(UserManager<BlogUser> userManager, ApplicationContext applicationContext)
        {
            _userManager = userManager;
            _applicationContext = applicationContext;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Posts = await _applicationContext.Posts
                .Include(b => b.Author)
                .ToListAsync();

            return Page();
        }
    }
}