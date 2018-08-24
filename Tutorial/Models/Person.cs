using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(100)]
        public string Lastname { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")]
        public string DoB { get; set; }
        public int Age { get; set; }
        [Required]
        [Range(1,500)]
        public Int16 Weight { get; set; }
        public Byte Enabled { get; set; }
        
        public Person()
        {

        }
    }
}
