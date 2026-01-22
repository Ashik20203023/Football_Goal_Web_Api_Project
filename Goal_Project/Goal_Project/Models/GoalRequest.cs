namespace Goal_Project.Models
{
    public class GoalRequest
    {
        public DateTime goal_time { get; set; } 
        public string team1 { get; set; } = null!;
        public string team2 { get; set; } = null!;
        public string goal_scorer { get; set; } = null!;
        public string? assisting_player { get; set; }
    }
}
