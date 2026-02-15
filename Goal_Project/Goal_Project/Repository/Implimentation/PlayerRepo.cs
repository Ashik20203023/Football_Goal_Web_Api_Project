using DocumentFormat.OpenXml.Spreadsheet;
using Goal_Project.Models;
using Goal_Project.Repository.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Runtime.Intrinsics.X86;

namespace Goal_Project.Repository.Implimentation
{
    public class PlayerRepo:IPlayerRepo
    {
        private readonly IMongoCollection<Player> _players;
        private readonly IMongoCollection<Team> _teams;

        public PlayerRepo(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _players = database.GetCollection<Player>("Players");
            _teams = database.GetCollection<Team>("Teams");
        }

        public Task<List<Player>>GetAllPlayers()
        {
            return _players.Find(Builders<Player>.Filter.Empty).ToListAsync();
        }

        public async Task<bool> PlayerIdAsync(string playerId)
        {
            var count = await _players.Find(p => p.PlayerId == playerId).CountDocumentsAsync();
            return count > 0;

        }

        public async Task<bool> PlayerNameAsync(string playerName)
        {
            var count = await _players.Find(p => p.PlayerName == playerName).CountDocumentsAsync();
            return count > 0;
        }

        public async Task InsertAsync(List<Player> players)
        {
            if (players == null || players.Count == 0)
                return;

            await _players.InsertManyAsync(players);
            
        }


        //  Get Players by TeamName
        public async Task<List<Player>> GetPlayersByTeamNameAsync(string teamName)
        {
            var team = await _teams.Find(t => t.TeamName == teamName).FirstOrDefaultAsync();
            if (team == null)
                return new List<Player>();

            var players = await _players.Find(p => p.TeamId == team.Id).ToListAsync();

            players.ForEach(p => p.TeamName = teamName);
            return players;

        }

        //  Get Player Details
        public async Task<Player?> GetPlayerByNameAsync(string playerName)
        {
            
            var player= await _players.Find(p=>p.PlayerName== playerName).FirstOrDefaultAsync();
            if(player == null) return null;

            if(!string.IsNullOrEmpty(player.TeamId))
            {
                var team= await _teams.Find(t=>t.Id== player.TeamId).FirstOrDefaultAsync();
                player.TeamName=team?.TeamName;
            }
            return player;
        }

        public async Task<List<Player>> GetPlayersByIdsAsync(List<string> id)
        {
            return await _players.Find(p => id.Contains(p.Id!)).ToListAsync();
            
            
        }

    }
}
