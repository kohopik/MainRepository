using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieProjects.Model;

namespace IndieProjects.Model
{
    public class Vakanci
    {
        public int ID { get; set; }

        public Project Project { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Responsibilities { get; set; }

        public string Requirements { get; set; }

        public TypeOfEmployment TypeOfEmployment { get; set; }

        public DateTime DateOfPublish { get; set; }

        public string Title { get; set; }

        public StatusVakancy StatusVakancy { get; set; }
    }

    public enum StatusVakancy
    {
        Active,
        Desable
    }

    public enum TypeOfEmployment
    {
        Full,
        NotFull,
        FarAway
    }
}
