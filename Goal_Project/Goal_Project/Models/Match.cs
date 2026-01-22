using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using System.ComponentModel.DataAnnotations;

namespace Goal_Project.Models
{
    public class Match
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [BsonRepresentation(BsonType.String)]
        public Guid MatchGuid { get; set; }

        [Required]
        public DateTime MatchStartTime { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Team1Id { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Team2Id { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? VenueId { get; set; }

        [Required]
        public List<string> Team1Players { get; set; } = new List<string>();

        [Required]
        public List<string> Team2Players { get; set; } = new List<string>();

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Team1CaptainId { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Team2CaptainId { get; set; }
    }

    
    
}
