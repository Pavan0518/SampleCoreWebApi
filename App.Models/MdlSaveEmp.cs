using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Models
{
    public class MdlSaveEmp
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="FName is mandatory"), MaxLength(50, ErrorMessage = "First name length should not exceed 50 characters.")]
        public string FName { get; set; }
        public string LName { get; set; }
        [Required(ErrorMessage = "Designation is mandatory")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Gender is mandatory"), MaxLength(1)]
        public string Gender { get; set; }
    }
}
