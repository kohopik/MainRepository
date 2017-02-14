using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IndieProjects.Model;

namespace IndieProjects.Model
{
    public class DeveloperProject
    {
        [Key]
        public int DeveloperProjectID { get; set; }
        public User User { get; set; }
        [ForeignKey("Project")]
        public int ProjectID { get; set; }
        public Project Project { get; set; }
    }
}
