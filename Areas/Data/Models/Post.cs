using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Areas.Data
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImgSrc { get; set; }
        public string ImgAlt { get; set; }
        public DateTime PublishTime { get; set; }
        public BlogUser Author { get; set; }
    }
}
