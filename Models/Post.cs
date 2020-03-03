using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImgSrc { get; set; }
        public string ImgAlt { get; set; }
        public DateTime PublishTime { get; set; }
    }
}
