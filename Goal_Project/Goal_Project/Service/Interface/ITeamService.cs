using Goal_Project.Models;

namespace Goal_Project.Service.Interface
{
    public interface ITeamService
    {
        Task<string> AddTeamAsync(Team team);
        Task<List<Team>> GetTeamsAsync();
    }
}
