using Goal_Project.Models;

namespace Goal_Project.Repository.Interface
{
    public interface ITeamRepo
    {
        Task<List<Team>> GetAllAsync();
        Task<bool> TeamNameAsync(string  teamName);

        Task<bool> TeamIdAsync(string teamId);
        Task InsertAsync(List<Team>teams);
        Task<string?> GetTeamMongoIdByNameAsync(string teamName);
    }
}
