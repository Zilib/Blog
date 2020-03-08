using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Blog.Areas.Blog.Pages
{
    public class EloModel : PageModel
    {
        private readonly ApplicationContext _applicationContext;
        public List<Post> Posts { get; set; }

        public EloModel(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Posts = await _applicationContext.Posts.ToListAsync();

            return Page();
        }
    }
}
