using Goal_Project.Models;
using Goal_Project.Repository.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Goal_Project.Repository.Implimentation
{
    public class TeamRepo:ITeamRepo
    {
        private readonly IMongoCollection<Team> _teams;

        public TeamRepo(IOptions<MongoSettings>settings)
        {
            var client=new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _teams = database.GetCollection<Team>("Teams");
        }

        public Task<List<Team>>GetAllAsync()
        {
            return _teams.Find(Builders<Team>.Filter.Empty).ToListAsync();

        }
        public async Task<bool> TeamNameAsync(string teamName)
        {
            var count= await _teams.Find(t=>t.TeamName==teamName).CountDocumentsAsync();
            return count > 0;
        }
        public async Task<bool> TeamIdAsync(string teamId)
        {
            var count=await _teams.Find(t=>t.TeamId==teamId).CountDocumentsAsync();
            return count > 0;
        }
        public async Task InsertAsync(List<Team>Teams)
        {
            if (Teams == null || Teams.Count() == 0)
                return;
            await _teams.InsertManyAsync(Teams);
        }
        public async Task<string?> GetTeamMongoIdByNameAsync(string teamName)
        {
            var team = await _teams.Find(t => t.TeamName == teamName).FirstOrDefaultAsync();
            return team?.Id;
        }
    }
}
