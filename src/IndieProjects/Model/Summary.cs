using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieProjects.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndieProjects.Model
{
    public class Summary
    {
        [Key]
        [Required]       
        public int UserID { get; set; }

        public User Developer { get; set; }

        public SummaryStatus SummaryStatus { get; set; }

        public string AboutHimself { get; set; }

        public string LinksToProject { get; set; }
    }

    public enum SummaryStatus
    {
        Active,
        Desable
    }
}
