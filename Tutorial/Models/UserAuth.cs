using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Tutorial.Models
{
    public class UserAuth
    {
        [Required]        
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
