using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndieProjects.Model
{
    public class Tag
    {
        public int TagId { get; set; }

        public string Name { get; set; }
    }
}
