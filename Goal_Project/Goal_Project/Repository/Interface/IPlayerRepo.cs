using Goal_Project.Models;

namespace Goal_Project.Repository.Interface
{
    public interface IPlayerRepo
    {
        Task<List<Player>> GetAllPlayers();
        Task<bool> PlayerIdAsync(string playerId);
        Task<bool> PlayerNameAsync(string playerName);
        Task InsertAsync(List<Player> players);
        Task<List<Player>> GetPlayersByTeamNameAsync(string teamName);
        Task<Player?> GetPlayerByNameAsync(string playerName);
        Task<List<Player>> GetPlayersByIdsAsync(List<string>id);
    }
}
