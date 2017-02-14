using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieProjects.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndieProjects.Model
{
    public class Project
    {
        public Project()
        {
            Commentaries = new List<Commentary>();
            Team = new List<DeveloperProject>();
            Vakancies = new List<Vakanci>();
        }
        public int ProjectID { get; set; }

        public string Avatar { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Links { get; set; }

        public ICollection<Commentary> Commentaries { get; set; }

        public int Likes { get; set; }

        public User ProjectManager { get; set; }

        public List<DeveloperProject> Team { get; set; }

        public ProjectStatus StatusProject { get; set; }

        public List<Vakanci> Vakancies { get; set; }
    }

    public enum ProjectStatus
    {
        Active,
        End
    }
}
