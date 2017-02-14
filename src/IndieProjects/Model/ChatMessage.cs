using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieProjects.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndieProjects.Model
{
    public class ChatMessage
    {
        public int ID { get; set; }

        public User Author { get; set; }

        public DateTime DateSend { get; set; }

        public Project Chat { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }
    }
}
