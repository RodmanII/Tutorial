using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.Utils;

namespace Tutorial.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMessages.required)]
        [StringLength(100, ErrorMessage = ErrorMessages.stringLength)]
        /*Name es el nombre de la propiedad en la clase indicada mediante ResourceType de la cual 
         se tomara el valor de localizacion, no es el texto que va a mostrar en lugar del nombre del campo, 
         eso lo define la propiedad Description*/
        [Display(Name = "Name", ResourceType = typeof(Localization.Person))]
        public string Firstname { get; set; }

        [Required(ErrorMessage = ErrorMessages.required)]
        [StringLength(100, ErrorMessage = ErrorMessages.stringLength)]
        [Display(Name = "Lastname", ResourceType = typeof(Localization.Person))]
        public string Lastname { get; set; }

        [Required(ErrorMessage = ErrorMessages.required)]
        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$", ErrorMessage = ErrorMessages.dateRegex)]        
        [Display(Name = "Dob", Description = "Date of birth", ResourceType = typeof(Localization.Person))]
        public string DoB { get; set; }

        public int Age { get; set; }

        [Required(ErrorMessage = ErrorMessages.required)]
        [Range(1,500, ErrorMessage = ErrorMessages.range)]
        [Display(Name = "Wei", ResourceType = typeof(Localization.Person))]
        public Int16 Weight { get; set; }

        public Byte Enabled { get; set; }
        
        public Person()
        {

        }
    }
}
