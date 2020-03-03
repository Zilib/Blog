using Blog.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public BlogUser Author { get; set; }
    }
}
