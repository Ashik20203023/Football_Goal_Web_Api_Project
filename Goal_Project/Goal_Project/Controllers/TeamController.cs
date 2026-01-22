using Goal_Project.Models;
using Goal_Project.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Goal_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddTeam([FromBody] Team team)
        {
            var result= await _teamService.AddTeamAsync(team);
            if(result!="Team Added")
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _teamService.GetTeamsAsync());
        }
    }
}
