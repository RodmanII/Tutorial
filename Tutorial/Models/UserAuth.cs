using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Tutorial.Utils;

namespace Tutorial.Models
{
    public class UserAuth
    {
        [Required(ErrorMessage = ErrorMessages.required)]
        [Display(Name = "User", ResourceType = typeof(Localization.UserAuth))]
        public string Username { get; set; }

        [Required(ErrorMessage = ErrorMessages.required)]
        [Display(Name = "Password", ResourceType = typeof(Localization.UserAuth))]
        public string Password { get; set; }
    }
}
