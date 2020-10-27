using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class Login
    {
      

        [Required(ErrorMessage = "Please Enter Email Id")]
        [Column(TypeName = "varchar(50)")]
        public string EmailId { get; set; }


        [Required(ErrorMessage = "Please Enter Password")]
        [Column(TypeName = "varchar(500)")]
        public string Password { get; set; }
    }
}
