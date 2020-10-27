using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class Movies
    {
        [BsonId]
        [JsonProperty("objectId")]
        public ObjectId ObjectId { get; set; }


        [Required(ErrorMessage = "Please Enter MovieName")]
        [Column(TypeName = "varchar(50)")]
        public string MovieName { get; set; }


        [Required(ErrorMessage = "Please Enter Genre")]
        [Column(TypeName = "varchar(500)")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Please Enter InitialRelease")]
        [Column(TypeName = "varchar(50)")]
        public string InitialRelease { get; set; }

        [Required(ErrorMessage = "Please Enter Director Name")]

        [Column(TypeName = "varchar(250)")]
        public string Director { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ImageURL { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }


    }
}
