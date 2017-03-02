using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndieProjects.Model
{
    public class Profile
    {
        public Profile()
        {
            Articles = new List<Article>();
            Projects = new List<DeveloperProject>();
            Messages = new List<Message>();
            Commentaries = new List<ProjectCommentaries>();
        }

        public User User { get; set; }

        public string NickName { get; set; }

        public string FIO { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Skype { get; set; }

        public string OwnSite { get; set; }

        public string Avatar { get; set; }

        public string AboutMe { get; set; }

        public UserStatus Status { get; set; }

        public List<Article> Articles { get; set; }

        public List<ProjectCommentaries> Commentaries { get; set; }

        public List<Message> Messages { get; set; }

        public string ThePost { get; set; }

        public Summary Summary { get; set; }

        public List<DeveloperProject> Projects { get; set; }
    }
}
