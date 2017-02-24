using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieProjects.Model;

namespace IndieProjects.Model
{
    public class ProjectCommentaries
    {
        public ProjectCommentaries()
        {
            ChildsCommentary = new List<ProjectCommentaries>();
        }

        public int ID { get; set; }

        public DateTime DateSend { get; set; }

        public string Content { get; set; }

        public User Author { get; set; }

        public ProjectCommentaries ParentCommentary { get; set; }

        public List<ProjectCommentaries> ChildsCommentary { get; set; }

        public Project Project { get; set; }
    }
}
