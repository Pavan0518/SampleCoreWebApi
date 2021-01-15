using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Repository
{
    public class Login
    {
        [Required(ErrorMessage ="Email is mandatory."), EmailAddress(ErrorMessage ="Please enter valid email id.")]
        public string email { get; set; }
        [Required(ErrorMessage ="Password is mandarory.")]
        public string password { get; set; }
    }
}
