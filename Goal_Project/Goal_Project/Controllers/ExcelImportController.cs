using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Goal_Project.Models;
using Goal_Project.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Goal_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelImportController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExcelImportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("venues")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadVenues( IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Excel file is empty");

            var venues = new List<Venue>();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new ClosedXML.Excel.XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);

            var range = worksheet.RangeUsed();
            if (range == null)
                return BadRequest("Excel file contains no data.");

            var rows = range.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                var venue = new Venue
                {
                    VenueId = row.Cell(1).GetString().Trim(),
                    VenueName = row.Cell(2).GetString().Trim(),
                    City = row.Cell(3).GetString().Trim(),
                    Country = row.Cell(4).GetString().Trim()
                };

                if (string.IsNullOrWhiteSpace(venue.VenueId) ||
                    string.IsNullOrWhiteSpace(venue.VenueName) ||
                    string.IsNullOrWhiteSpace(venue.City) ||
                    string.IsNullOrWhiteSpace(venue.Country))
                {
                    return BadRequest("All fields are required in Excel file.");
                }

                venues.Add(venue);
            }

            // Validate BEFORE inserting anything
            foreach (var venue in venues)
            {
                if (await _unitOfWork.VenueRepo.VenueIdAsync(venue.VenueId!))
                    return BadRequest($"VenueId already exists: {venue.VenueId}");

                if (await _unitOfWork.VenueRepo.VenueNameAsync(venue.VenueName!))
                    return BadRequest($"VenueName already exists: {venue.VenueName}");
            }

            await _unitOfWork.VenueRepo.InsertAsync(venues);
            return Ok("Venues uploaded successfully");
        }


        [HttpPost("teams")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadTeams(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Excel file is empty");

            var teams = new List<Team>();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);

            var range = worksheet.RangeUsed();
            if (range == null)
                return BadRequest("Excel file contains no data");

            // Skip header row
            var rows = range.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                var team = new Team
                {
                    TeamId = row.Cell(1).GetString().Trim(),
                    TeamName = row.Cell(2).GetString().Trim(),
                    City = row.Cell(3).GetString().Trim(),
                    Country = row.Cell(4).GetString().Trim()
                };

                // All fields required
                if (string.IsNullOrWhiteSpace(team.TeamId) ||
                    string.IsNullOrWhiteSpace(team.TeamName) ||
                    string.IsNullOrWhiteSpace(team.City) ||
                    string.IsNullOrWhiteSpace(team.Country))
                {
                    return BadRequest("All fields are required in Excel file");
                }

                teams.Add(team);
            }

            //  VALIDATION BEFORE INSERT 
            foreach (var team in teams)
            {
                if (await _unitOfWork.TeamRepo.TeamIdAsync(team.TeamId!))
                    return BadRequest($"TeamId already exists: {team.TeamId}");

                if (await _unitOfWork.TeamRepo.TeamNameAsync(team.TeamName!))
                    return BadRequest($"TeamName already exists: {team.TeamName}");
            }

            // Insert only if all validations pass
            await _unitOfWork.TeamRepo.InsertAsync(teams);

            return Ok("Teams uploaded successfully");
        }

        [HttpPost("players")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadPlayers(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Excel file is empty");

            var players = new List<Player>();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);

            var range = worksheet.RangeUsed();
            if (range == null)
                return BadRequest("Excel file contains no data");

            var rows = range.RowsUsed().Skip(1); // Skip header

            foreach (var row in rows)
            {
                var player = new Player
                {
                    PlayerId = row.Cell(1).GetString().Trim(),
                    PlayerName = row.Cell(2).GetString().Trim(),
                    Position = row.Cell(3).GetString().Trim(),
                    TeamName = row.Cell(4).GetString().Trim() // Excel has TeamName
                };

                // All fields required
                if (string.IsNullOrWhiteSpace(player.PlayerId) ||
                    string.IsNullOrWhiteSpace(player.PlayerName) ||
                    string.IsNullOrWhiteSpace(player.Position) ||
                    string.IsNullOrWhiteSpace(player.TeamName))
                {
                    return BadRequest("All player fields are required in Excel file");
                }

                players.Add(player);
            }

            //  VALIDATIONS BEFORE INSERT 

            var validPositions = new List<string>
              {
                 "Goalkeeper",
                 "Defender",
                 "Midfielder",
                 "Forward"
             };

            foreach (var player in players)
            {
                // Position validation
                if (!validPositions.Contains(player.Position!))
                    return BadRequest($"Invalid position: {player.Position}");

                // PlayerId Duplicate
                if (await _unitOfWork.PlayerRepo.PlayerIdAsync(player.PlayerId!))
                    return BadRequest($"PlayerId already exists: {player.PlayerId}");

                // PlayerName Duplicate
                if (await _unitOfWork.PlayerRepo.PlayerNameAsync(player.PlayerName!))
                    return BadRequest($"PlayerName already exists: {player.PlayerName}");

                
                var teamMongoId =
                    await _unitOfWork.TeamRepo.GetTeamMongoIdByNameAsync(player.TeamName!);

                if (teamMongoId == null)
                    return BadRequest($"Team not found: {player.TeamName}");

                // Assign TeamId and remove TeamName
                player.TeamId = teamMongoId;
                player.TeamName = null;
            }

             
            await _unitOfWork.PlayerRepo.InsertAsync(players);

            return Ok("Players uploaded successfully");
        }

        
}

}
