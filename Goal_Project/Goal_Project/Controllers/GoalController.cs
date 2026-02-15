using Goal_Project.Models;
using Goal_Project.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Goal_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;

        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddGoal([FromBody] GoalRequest request)
        {
            var result = await _goalService.AddGoalAsync(request);

            if (result != "Goal added successfully")
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("All")]

        public async Task<IActionResult> GetAll()
        {
            return Ok(await _goalService.GetAllGoalsAsync());
        }

        [HttpGet("MatchSummary")]
        public async Task<IActionResult> GetMatchSummary([FromQuery] string team1, [FromQuery] string team2)
        {
            var result = await _goalService.GetMatchSummaryAsync(team1, team2);

            if (result is string) // error message
                return NotFound(result);

            return Ok(result);
        }
    }
}
