using Goal_Project.Models;
namespace Goal_Project.Repository.Interface
{
    public interface IMatchRepo
    {
        Task InsertAsync(Match match);
        Task<List<Match>> GetAllAsync();
        Task<Match?> GetLatestMatchAsync(string team1Id, string team2Id);


    }
}
