using Goal_Project.Models;
using Goal_Project.Repository.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Goal_Project.Repository.Implimentation
{
    public class MatchRepo : IMatchRepo
    {
        private readonly IMongoCollection<Match> _matches;

        public MatchRepo(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _matches = database.GetCollection<Match>("Matches");
        }

        public async Task InsertAsync(Match match)
        {
            await _matches.InsertOneAsync(match);
        }

        public Task<List<Match>> GetAllAsync()
        {
            return _matches.Find(Builders<Match>.Filter.Empty).ToListAsync();
        }
        public async Task<Match?> GetLatestMatchAsync(string team1Id, string team2Id)
        {
            return await _matches.Find(m =>(m.Team1Id == team1Id && m.Team2Id == team2Id) ||
                                      (m.Team1Id == team2Id && m.Team2Id == team1Id))
                                      .SortByDescending(m => m.MatchStartTime).FirstOrDefaultAsync();
        }

    }
}
