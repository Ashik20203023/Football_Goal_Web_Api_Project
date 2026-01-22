using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Goal_Project.Models
{
    public class Goal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [BsonRepresentation(BsonType.String)]
        public Guid GoalGuid { get; set; }

        [Required]
        [BsonRepresentation(BsonType.String)]
        public Guid MatchGuid { get; set; }

        [Required]
        public DateTime GoalTime { get; set; } 

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string GoalScorerId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? AssistPlayerId { get; set; }
    }
}
