using Goal_Project.Models;
using Goal_Project.Service.Interface;
using Goal_Project.UnitOfWork;

namespace Goal_Project.Service.Implimentation
{
    public class TeamService:ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TeamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> AddTeamAsync(Team team)
        {
            if (team == null)
                return "Team cannot be null";
            if (string.IsNullOrEmpty(team.TeamId)||
                string.IsNullOrEmpty(team.TeamName)||
                string.IsNullOrEmpty(team.City)||
                string.IsNullOrEmpty(team.Country))
                {
                return "All Teams are required";
            }

            if (await _unitOfWork.TeamRepo.TeamIdAsync(team.TeamId))
                return $"TeamId is already exists{team.TeamId}";

            if (await _unitOfWork.TeamRepo.TeamNameAsync(team.TeamName))
                return $"TeameName already exists{team.TeamName}";

            await _unitOfWork.TeamRepo.InsertAsync(new List<Team> { team});
            return "Team Added";

        }

        public async Task<List<Team>> GetTeamsAsync()
        {
           return await _unitOfWork.TeamRepo.GetAllAsync();
        }
    }
}
