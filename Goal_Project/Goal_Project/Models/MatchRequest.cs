using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
namespace Goal_Project.Models
{
    public class MatchRequest
    {
        public DateTime match_start_time { get; set; }
        public string? venu_name { get; set; }
        public string? team1 { get; set; }
        public string? team2 { get; set; }
        public List<string>? team1_players { get; set; }
        public List<string>? team2_players { get; set; }
        public string? team1_captain { get; set; }
        public string? team2_captain { get; set; }
    }
}
