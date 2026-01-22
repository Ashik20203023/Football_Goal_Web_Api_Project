using Goal_Project.Models;
using Goal_Project.Repository.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Goal_Project.Repository.Implimentation
{
    public class GoalRepo : IGoalRepo
    {
        private readonly IMongoCollection<Goal> _goals;

        public GoalRepo(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _goals = database.GetCollection<Goal>("Goals");
        }
        public async Task<List<Goal>> GetAllAsync()
        {
            return await _goals.Find(Builders<Goal>.Filter.Empty).ToListAsync();
        }

        public async Task InsertAsync(Goal goal)
        {
            await _goals.InsertOneAsync(goal);
        }
        public async Task<List<Goal>> GetGoalsByMatchGuidAsync(Guid matchGuid)
        {
            return await _goals.Find(g => g.MatchGuid == matchGuid).ToListAsync();
        }
    }
}
