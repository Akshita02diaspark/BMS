using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class Theater
    {
        [BsonId]
        [JsonProperty("objectId")]
        public ObjectId ObjectId { get; set; }
        
        [JsonProperty("MovieId")]
        public string MovieId { get; set; }


        public string ReservedSeats { get; set; }
    }
}
