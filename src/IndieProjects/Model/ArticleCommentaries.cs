using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndieProjects.Model
{
    public class ArticleCommentaries
    {
        public ArticleCommentaries()
        {
            ChildsCommentary = new List<ArticleCommentaries>();
        }

        public int ID { get; set; }

        public DateTime DateSend { get; set; }

        public string Content { get; set; }

        public User Author { get; set; }

        public ArticleCommentaries ParentCommentary { get; set; }

        public List<ArticleCommentaries> ChildsCommentary { get; set; }

        public Article Article { get; set; }
    }
}
