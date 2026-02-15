using Goal_Project.Models;
using Goal_Project.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Goal_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        
        [HttpPost("Add")]
        public async Task<IActionResult> AddMatch([FromBody] MatchRequest request)
        {
            if (request == null)
                return BadRequest("Match request cannot be null");

            var result = await _matchService.AddMatchAsync(request);

            if (result != "Match added successfully")
                return BadRequest(result);

            return Ok(result);
        }

        
        [HttpGet("All")]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _matchService.GetAllMatchesAsync();
            return Ok(matches);
        }

        [HttpGet("Formation")]
        public async Task<IActionResult> GetFormation([FromQuery] string team1, [FromQuery] string team2)
        {
            var result = await _matchService.GetMatchFormationAsync(team1, team2);

            if (result is string) 
                return NotFound(result);

            return Ok(result);
        }


    }
}
