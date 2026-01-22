//using ClosedXML.Excel;
using Goal_Project.Models;
using Goal_Project.Repository.Interface;
using Goal_Project.Service.Interface;
using Goal_Project.UnitOfWork;


namespace Goal_Project.Service.Implimentation
{
    public class VenueService:IVenueService
    {
        private readonly IUnitOfWork _unitOfWork;
        public VenueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public async Task<string> AddVenueAsync(Venue venue)
        {
            if (venue == null)
                return "Venue cannot be null";

            if (string.IsNullOrWhiteSpace(venue.VenueId) ||
                string.IsNullOrWhiteSpace(venue.VenueName) ||
                string.IsNullOrWhiteSpace(venue.City) ||
                string.IsNullOrWhiteSpace(venue.Country))
            {
                return "All fields are required";
            }
            if (await _unitOfWork.VenueRepo.VenueIdAsync(venue.VenueId))
                return $"VenueId already exists{venue.VenueId}";

            
            if (await _unitOfWork.VenueRepo.VenueNameAsync(venue.VenueName))
                return $"Venue name already exists: {venue.VenueName}";

           

            await _unitOfWork.VenueRepo.InsertAsync(new List<Venue> { venue });
            return "Venue Added";
        }

        public async Task<List<Venue>> GetAllAsync()
        {
            return await _unitOfWork.VenueRepo.GetAllAsync();
        }

        

    }
}
