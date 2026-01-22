using Goal_Project.Models;

namespace Goal_Project.Service.Interface
{
    public interface IPlayerService
    {
        Task<string> AddPlayersAsync(List<Player> players);
        Task<List<Player>> GetAllPlayerAsync();
        
        Task<object> GetSquadInfoAsync(string teamName);

        
        Task<object?> GetPlayerDetailsAsync(string playerName);
    }
}
