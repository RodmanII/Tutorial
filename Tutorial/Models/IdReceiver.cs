using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.Utils;

namespace Tutorial.Models
{
    public class IdReceiver
    {
        [Required(ErrorMessage = ErrorMessages.required)]
        [Range(1, Int32.MaxValue, ErrorMessage = ErrorMessages.range)]
        public int Id { get; set; }
    }
}
