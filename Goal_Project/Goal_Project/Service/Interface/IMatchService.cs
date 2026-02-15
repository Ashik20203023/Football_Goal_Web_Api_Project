using Goal_Project.Models;

namespace Goal_Project.Service.Interface
{
    public interface IMatchService
    {
        Task<string> AddMatchAsync(MatchRequest request);
        Task<List<object>> GetAllMatchesAsync();
        Task<object> GetMatchFormationAsync(string team1Name, string team2Name);
    }
}
