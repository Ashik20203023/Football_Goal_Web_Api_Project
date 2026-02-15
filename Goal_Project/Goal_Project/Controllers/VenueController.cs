using Goal_Project.Models;
using Goal_Project.Service.Interface;

using Microsoft.AspNetCore.Mvc;

namespace Goal_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    //[Route("api/[controller]")]
    //[ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService)
        {
            _venueService = venueService;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddVenue([FromBody] Venue venue)
        {
            var result = await _venueService.AddVenueAsync(venue);

            if (result != "Venue Added")
                return BadRequest(result);

            return Ok(result);
        }

        


        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _venueService.GetAllAsync());
        }
    }
}
