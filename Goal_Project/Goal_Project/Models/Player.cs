using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Goal_Project.Models
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        [Required]
        public string? PlayerId { get; set; }
        [Required]
        public string? PlayerName { get; set; }
        [Required]

        public string? Position { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? TeamId { get; set; }

        [BsonIgnore]
        [Required]
        public string? TeamName { get; set; }
    }
}
