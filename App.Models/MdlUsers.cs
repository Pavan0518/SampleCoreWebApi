using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Models
{
    public class MdlUsers
    {
        public string user_id { get; set; }
        public int id { get; set; }
        [Required(ErrorMessage ="Fist Name is mandatory.")]
        public string first_name { get; set; }
        [Required(ErrorMessage = "Last Name is mandatory.")]
        public string last_name { get; set; }
        [Required(ErrorMessage = "Email is mandatory."), EmailAddress(ErrorMessage ="Email format incorrect.")]
        public string email { get; set; }
        public string phone { get; set; }
        [Required(ErrorMessage = "Password is mandatory.")]
        public string password { get; set; }
        [Required(ErrorMessage = "IsActive is mandatory.")]
        public bool isActive { get; set; }

    }
}
