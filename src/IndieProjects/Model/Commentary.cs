using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieProjects.Model;

namespace IndieProjects.Model
{
    public class Commentary
    {
        public int ID { get; set; }

        public DateTime DateSend { get; set; }

        public string Content { get; set; }

        public User Author { get; set; }

        public Commentary SelfCommentary { get; set; }

        public Article Article { get; set; }

        public Project Project { get; set; }
    }
}
