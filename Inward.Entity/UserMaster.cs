using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inward.Entity
{
    public class UserMaster
    {
        public long UserId { get; set; }


        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "UserName")]
        [StringLength(100, ErrorMessage = "Please enter valid user")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        //[RegularExpression("^(?=.*[0-9])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*0-9]{8,}$", ErrorMessage = "The Password must have minimum 8 characters and at least one numeric and one special character")]
        [StringLength(15, ErrorMessage = "Please enter correct password")]
        public string Password { get; set; } = null!;
    }
}
