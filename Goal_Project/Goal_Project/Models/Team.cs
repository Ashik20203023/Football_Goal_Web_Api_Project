using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Goal_Project.Models
{
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        [Required]
        public string? TeamId { get; set; }
        [Required]
        public string? TeamName { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string ?Country { get; set; }
    }
}
