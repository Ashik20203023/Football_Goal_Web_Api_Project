using Goal_Project.Models;
using Goal_Project.Service.Interface;
using Goal_Project.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Goal_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService; 

        public PlayerController(IPlayerService playerService)
        {
            _playerService= playerService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddPlayers([FromBody] List<Player> players)
        {
            var result = await _playerService.AddPlayersAsync(players);

            if (result != "Players Added Successfully")
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _playerService.GetAllPlayerAsync());
        }

        // Get Squad Information
        [HttpGet("squad/{teamName}")]
        public async Task<IActionResult> GetSquadInfo(string teamName)
        {
            var result = await _playerService.GetSquadInfoAsync(teamName);

            if (result is string)
                return NotFound(result);

            return Ok(result);
        }

        //  Get Player Details
        [HttpGet("details/{playerName}")]
        public async Task<IActionResult> GetPlayerDetails(string playerName)
        {
            var result = await _playerService.GetPlayerDetailsAsync(playerName);

            if (result == null)
                return NotFound("Player not found");

            return Ok(result);
        }
    }
}
