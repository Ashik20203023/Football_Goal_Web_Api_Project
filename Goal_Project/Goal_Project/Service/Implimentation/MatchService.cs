using Goal_Project.Models;
using Goal_Project.Service.Interface;
using Goal_Project.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goal_Project.Service.Implimentation
{
    public class MatchService : IMatchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MatchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> AddMatchAsync(MatchRequest request)
        {
            // Validate Venue
            var venueId = await _unitOfWork.VenueRepo.GetVenueMongoIdByNameAsync(request.venu_name);
                if (venueId == null) 
                return $"Venue not found: {request.venu_name}";

            // Validate Teams
            var team1Id = await _unitOfWork.TeamRepo.GetTeamMongoIdByNameAsync(request.team1);
                if (team1Id == null) 
                return $"Team not found: {request.team1}";

            var team2Id = await _unitOfWork.TeamRepo.GetTeamMongoIdByNameAsync(request.team2);
            if (team2Id == null) return $"Team not found: {request.team2}";

            // Validate Players count
            if (request.team1_players == null || request.team1_players.Count != 11)
                return "Team1 must have exactly 11 players";
            if (request.team2_players == null || request.team2_players.Count != 11)
                return "Team2 must have exactly 11 players";

            // Get Player IDs
            var team1PlayerIds = new List<string>();
            foreach (var playerName in request.team1_players)
            {
                var player = await _unitOfWork.PlayerRepo.GetPlayerByNameAsync(playerName);
                if (player == null) return $"Player not found in team1: {playerName}";
                team1PlayerIds.Add(player.Id!);
            }

            if (team1PlayerIds.Distinct().Count() != 11)
                return "Team1 contains duplicate players";

            var team2PlayerIds = new List<string>();
            foreach (var playerName in request.team2_players)
            {
                var player = await _unitOfWork.PlayerRepo.GetPlayerByNameAsync(playerName);
                if (player == null) return $"Player not found in team2: {playerName}";
                team2PlayerIds.Add(player.Id!);
            }
            if (team2PlayerIds.Distinct().Count() != 11)
                return "Team2 contains duplicate players";

            //  Get Captain IDs
            var team1Captain = await _unitOfWork.PlayerRepo.GetPlayerByNameAsync(request.team1_captain);
            if (team1Captain == null) return $"Team1 captain not found: {request.team1_captain}";
            if (!team1PlayerIds.Contains(team1Captain.Id!))
                return "Team1 captain must be in playing XI";

            var team2Captain = await _unitOfWork.PlayerRepo.GetPlayerByNameAsync(request.team2_captain);
            if (team2Captain == null) return $"Team2 captain not found: {request.team2_captain}";
            if (!team2PlayerIds.Contains(team2Captain.Id!))
                return "Team2 captain must be in playing XI";

            // Create Match
            var match = new Match
            {
                MatchGuid = Guid.NewGuid(),
                MatchStartTime = request.match_start_time,
                VenueId = venueId,
                Team1Id = team1Id,
                Team2Id = team2Id,
                Team1Players = team1PlayerIds,
                Team2Players = team2PlayerIds,
                Team1CaptainId = team1Captain.Id,
                Team2CaptainId = team2Captain.Id
            };

            // Insert into DB
            await _unitOfWork.MatchRepo.InsertAsync(match);

            return "Match added successfully";
        }

        public async Task<List<Match>> GetAllAsync()
        {
            
            return await _unitOfWork.MatchRepo.GetAllAsync();
        }

        public async Task<object> GetMatchFormationAsync(string team1Name, string team2Name)
        {
            // Get Team IDs
            var team1Id = await _unitOfWork.TeamRepo.GetTeamMongoIdByNameAsync(team1Name);
            var team2Id = await _unitOfWork.TeamRepo.GetTeamMongoIdByNameAsync(team2Name);

            if (team1Id == null || team2Id == null)
                return "One or both teams not found";

            // Get latest match between these teams
            var match = await _unitOfWork.MatchRepo.GetLatestMatchAsync(team1Id, team2Id);
            if (match == null)
                return "No match found between these teams";

            // Get player objects for both teams
            var team1Players = await _unitOfWork.PlayerRepo.GetPlayersByIdsAsync(match.Team1Players);
            var team2Players = await _unitOfWork.PlayerRepo.GetPlayersByIdsAsync(match.Team2Players);

            // Count positions for formation
            string GetFormation(List<Player> players)
            {
                int goalkeeper = players.Count(p => p.Position == "Goalkeeper");
                int defender = players.Count(p => p.Position == "Defender");
                int midfielder = players.Count(p => p.Position == "Midfielder");
                int forward = players.Count(p => p.Position == "Forward");
                return $"{defender}-{midfielder}-{forward}";
            }

            // Return result
            return new
            {
                Team1 = team1Name,
                Formation1 = GetFormation(team1Players),
                Team2 = team2Name,
                Formation2 = GetFormation(team2Players)
            };
        }




    }
}
