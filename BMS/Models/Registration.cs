using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class Registration
    {
        [BsonId]
        [JsonProperty("objectId")]
        public ObjectId ObjectId { get; set; }

        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter FirstName")]
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Please Enter Name")]
        [Column(TypeName = "varchar(500)")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile no.")]
        [Column(TypeName = "varchar(50)")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]

        [Column(TypeName = "varchar(250)")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]

        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

        public string ConfirmPassword { get; set; }
    }
}
