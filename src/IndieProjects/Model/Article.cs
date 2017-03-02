using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieProjects.Model;

namespace IndieProjects.Model
{
    public class Article
    {
        public Article()
        {
            Tags = new List<Tag>();
            Commentaries = new List<ArticleCommentaries>();
            Likes = new List<Like>();
        }
        public int ID { get; set; }

        public User Author { get; set; }

        public string ArticleAvatar { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DateOfPublish { get; set; }

        public List<Like> Likes { get; set; }

        public ICollection<ArticleCommentaries> Commentaries { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}
