using Goal_Project.Models;
using Goal_Project.Service.Interface;
using Goal_Project.UnitOfWork;

namespace Goal_Project.Service.Implimentation
{
    public class PlayerService:IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;

        
        private readonly List<string> ValidPositions = new()
        {
            "Goalkeeper",
            "Defender",
            "Midfielder",
            "Forward"
        };

        public PlayerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        

        public async Task<string> AddPlayersAsync(List<Player> players)
        {
            if (players == null || players.Count == 0)
                return "Player list cannot be empty";

            foreach (var player in players)
            {
                
                if (string.IsNullOrEmpty(player.PlayerId) ||
                    string.IsNullOrEmpty(player.PlayerName) ||
                    string.IsNullOrEmpty(player.Position) ||
                    string.IsNullOrEmpty(player.TeamName))
                {
                    return "All player fields are required";
                }

               

                if (!ValidPositions.Contains(player.Position))
                    return $"Invalid Positions{player.Position}";

                
                if (await _unitOfWork.PlayerRepo.PlayerIdAsync(player.PlayerId))
                    return $"PlayerId already exists: {player.PlayerId}";

                
                if (await _unitOfWork.PlayerRepo.PlayerNameAsync(player.PlayerName))
                    return $"PlayerName already exists: {player.PlayerName}";

                
                var teamMongoId= await _unitOfWork.TeamRepo.GetTeamMongoIdByNameAsync(player.TeamName);
                if (teamMongoId == null)
                    return $"Team not found:{player.TeamName}";
                
                player.TeamId = teamMongoId;
            }

            await _unitOfWork.PlayerRepo.InsertAsync(players);
            return "Players Added Successfully";
        }

        public  Task<List<Player>> GetAllPlayerAsync()
        {
          return   _unitOfWork.PlayerRepo.GetAllPlayers();
        }

        public async Task<object> GetSquadInfoAsync(string teamName)
        {
            
            var players= await _unitOfWork.PlayerRepo.GetPlayersByTeamNameAsync(teamName);
            if(players.Count==0)
            {
                return "Team not found or no players";
            }

            return new
            {
                Goalkeepers = players.Count(p => p.Position == "Goalkeeper"),
                Defenders = players.Count(p => p.Position == "Defender"),
                Midfielders = players.Count(p => p.Position == "Midfielder"),
                Forwards = players.Count(p => p.Position == "Forward")
            };
        }
        public async Task<object?> GetPlayerDetailsAsync(string playerName)
        {
            var player = await _unitOfWork.PlayerRepo.GetPlayerByNameAsync(playerName);
            if (player == null) return null;

            return new
            {
                player.PlayerName,
                player.PlayerId,
                player.Position,
                TeamName = player.TeamName
            };
        }
    }
}
