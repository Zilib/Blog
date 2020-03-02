using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            List<Post> Posts = new List<Post>()
            {
                new Post() {
                    Name = "Lorem Ipsum",
                    Content = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus aliquam vestibulum fringilla. Curabitur tincidunt dui ut tristique laoreet. Aenean consequat mattis mi, nec dapibus velit sodales quis. Praesent eget faucibus erat, quis fringilla nulla. Nunc a risus mattis, vulputate leo a, ornare ipsum. Vestibulum augue arcu, sollicitudin eu orci at, venenatis sollicitudin risus. Phasellus semper aliquet risus eu efficitur. Donec finibus nunc sed congue molestie. Integer placerat efficitur auctor.</p><p>Nullam placerat ullamcorper porttitor. Etiam commodo cursus luctus. Proin hendrerit, orci sed malesuada aliquam, risus justo tempor erat, sit amet rutrum elit neque sit amet sem. Integer vel rhoncus erat. Phasellus egestas lobortis enim, vitae congue nunc hendrerit id. Maecenas ultrices diam in leo convallis iaculis. Proin et commodo massa, eget vehicula nunc. Cras tempus nisi varius egestas dignissim. Praesent porta lacus ut elit euismod mattis. Aliquam non metus at mauris sollicitudin vestibulum. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Suspendisse aliquam dignissim aliquet.</p>",
                    PublishTime = DateTime.Now
                }
            };
            return View("HomePage", Posts);
        }
    }
}