using Goal_Project.Models;

namespace Goal_Project.Service.Interface
{
    public interface IGoalService
    {
        Task<string> AddGoalAsync(GoalRequest request);
        Task<List<Goal>> GetAllAsync();
        Task<object> GetMatchSummaryAsync(string team1Name, string team2Name);
    }
}
