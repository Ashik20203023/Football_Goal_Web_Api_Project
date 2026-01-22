using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Goal_Project.Models
{
    public class Venue
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        [Required]
        public string? VenueId { get; set; }

        [Required]
        public string? VenueName { get; set; } 
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Country { get; set; }

    }
}
