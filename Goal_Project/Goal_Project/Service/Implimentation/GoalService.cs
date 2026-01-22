using Goal_Project.Models;
using Goal_Project.Service.Interface;
using Goal_Project.UnitOfWork;

namespace Goal_Project.Service.Implimentation
{
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GoalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> AddGoalAsync(GoalRequest request)
        {
            //  Get team IDs
            var team1Id = await _unitOfWork.TeamRepo.GetTeamMongoIdByNameAsync(request.team1);
            var team2Id = await _unitOfWork.TeamRepo.GetTeamMongoIdByNameAsync(request.team2);

            if (team1Id == null || team2Id == null)
                return "Team not found";

            //  Get latest match between these teams
            var matches = await _unitOfWork.MatchRepo.GetAllAsync();

            var match = matches
                .Where(m =>
                    (m.Team1Id == team1Id && m.Team2Id == team2Id) ||
                    (m.Team1Id == team2Id && m.Team2Id == team1Id))
                .OrderByDescending(m => m.MatchStartTime)
                .FirstOrDefault();

            if (match == null)
                return "No match found between given teams";

            //  Validate goal time
            var matchStart = match.MatchStartTime;
            var matchEnd = matchStart.AddMinutes(140);

            if (request.goal_time <= matchStart)
                return "Goal time must be after match start time";

            if (request.goal_time >= matchEnd)
                return "Goal time must be before match end time";

            //  Get scorer
            var scorer = await _unitOfWork.PlayerRepo.GetPlayerByNameAsync(request.goal_scorer);
            if (scorer == null)
                return "Goal scorer not found";

            //  Assist validation
            string? assistId = null;

            if (!string.IsNullOrWhiteSpace(request.assisting_player))
            {
                var assist = await _unitOfWork.PlayerRepo.GetPlayerByNameAsync(request.assisting_player);
                if (assist == null)
                    return "Assisting player not found";

                if (assist.Id == scorer.Id)
                    return "Goal scorer and assisting player cannot be same";

                assistId = assist.Id;
            }

            //  Create Goal
            var goal = new Goal
            {
                GoalGuid = Guid.NewGuid(),
                MatchGuid = match.MatchGuid,
                GoalTime = request.goal_time,
                GoalScorerId = scorer.Id!,
                AssistPlayerId = assistId
            };

            await _unitOfWork.GoalRepo.InsertAsync(goal);

            return "Goal added successfully";
        }
        public async Task<List<Goal>> GetAllAsync()
        {
           return await _unitOfWork.GoalRepo.GetAllAsync();
        }

        public async Task<object> GetMatchSummaryAsync(string team1Name, string team2Name)
        {
            // Get team ID
            var team1Id = await _unitOfWork.TeamRepo.GetTeamMongoIdByNameAsync(team1Name);
            var team2Id = await _unitOfWork.TeamRepo.GetTeamMongoIdByNameAsync(team2Name);
            if (team1Id == null || team2Id == null)
                return "One or both teams not found";

            //  Get latest match
            var match = await _unitOfWork.MatchRepo.GetLatestMatchAsync(team1Id, team2Id);
            if (match == null)
                return "No match found between these teams";

            //  Get goals for the match
            var goals = await _unitOfWork.GoalRepo.GetGoalsByMatchGuidAsync(match.MatchGuid);

            //  Calculate team scores and goal details
            int team1Score = 0;
            int team2Score = 0;

            var goalDetails = new List<object>();
            foreach (var goal in goals)
            {
                var scorer = (await _unitOfWork.PlayerRepo.GetPlayersByIdsAsync(new List<string> { goal.GoalScorerId })).FirstOrDefault();
                var assist = goal.AssistPlayerId != null
                    ? (await _unitOfWork.PlayerRepo.GetPlayersByIdsAsync(new List<string> { goal.AssistPlayerId })).FirstOrDefault()
                    : null;

                if (scorer != null)
                {
                    if (match.Team1Players.Contains(scorer.Id)) team1Score++;
                    else if (match.Team2Players.Contains(scorer.Id)) team2Score++;
                }

                goalDetails.Add(new
                {
                    GoalTime = goal.GoalTime,
                    Scorer = scorer?.PlayerName,
                    Assist = assist?.PlayerName
                });
            }

            
            return new
            {
                MatchGuid = match.MatchGuid,
                MatchStartTime = match.MatchStartTime,
                Team1 = team1Name,
                Team2 = team2Name,
                Team1_Score = team1Score,
                Team2_Score = team2Score,
                Goals = goalDetails
            };
        }



    }
}
