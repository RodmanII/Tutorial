using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DoB { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public int Enabled { get; set; }
        
        public Person()
        {

        }
    }
}
