using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndieProjects.Model
{
    public class Like
    {
        public int ID { get; set; }

        public Article Article { get; set; }

        public Project Project { get; set; }

        public User User { get; set; }
    }
}
