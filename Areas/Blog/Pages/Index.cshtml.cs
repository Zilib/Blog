using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Blog.Areas.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blog.Areas.Blog
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<BlogUser> _userManager;
        public List<Post> Posts { get; set; }
        public class Post
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string ImgSrc { get; set; }
            public string ImgAlt { get; set; }
            public DateTime PublishTime { get; set; }
            public string AuthorId { get; set; }
            [ForeignKey("AuthorId")]
            public BlogUser Author { get; set; }
        }

        public IndexModel(UserManager<BlogUser> userManager)
        {
            _userManager = userManager;

        }
        public async Task<IActionResult> OnGetAsync()
        {
            Posts = new List<Post>()
            {
                new Post() {
                    Title = "Lorem Ipsum",
                    Content = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus aliquam vestibulum fringilla. Curabitur tincidunt dui ut tristique laoreet. Aenean consequat mattis mi, nec dapibus velit sodales quis. Praesent eget faucibus erat, quis fringilla nulla. Nunc a risus mattis, vulputate leo a, ornare ipsum. Vestibulum augue arcu, sollicitudin eu orci at, venenatis sollicitudin risus. Phasellus semper aliquet risus eu efficitur. Donec finibus nunc sed congue molestie. Integer placerat efficitur auctor.</p><p>Nullam placerat ullamcorper porttitor. Etiam commodo cursus luctus. Proin hendrerit, orci sed malesuada aliquam, risus justo tempor erat, sit amet rutrum elit neque sit amet sem. Integer vel rhoncus erat. Phasellus egestas lobortis enim, vitae congue nunc hendrerit id. Maecenas ultrices diam in leo convallis iaculis. Proin et commodo massa, eget vehicula nunc. Cras tempus nisi varius egestas dignissim. Praesent porta lacus ut elit euismod mattis. Aliquam non metus at mauris sollicitudin vestibulum. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Suspendisse aliquam dignissim aliquet.</p>",
                    ImgSrc = "~/img/Posts/PostExample.jpg",
                    Author = await _userManager.FindByIdAsync("1e87d7d8-7932-4286-9567-087409231a60"),
                    PublishTime = DateTime.Now
                }
            };

            return Page();
        }
    }
}