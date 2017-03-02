using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IndieProjects.Model;

namespace IndieProjects.Model
{
    public class Message
    {
        [Key]
        public int ID { get; set; }

        public DateTime DateOfSend { get; set; }

        public string Content { get; set; }

        public User Author { get; set; }

        public string Getter { get; set; }

        public MessageStatus MessageStatus { get; set; }
    }

    public enum MessageStatus
    {
        Read,
        NotRead,
        Error
    }
}
