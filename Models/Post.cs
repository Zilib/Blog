using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Post
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime PublishTime { get; set; }
    }
}
