using Goal_Project.Models;

namespace Goal_Project.Repository.Interface
{
    public interface IVenueRepo
    {
        Task<List<Venue>> GetAllAsync();
        Task<bool> VenueNameAsync(string venueName);
        Task<bool> VenueIdAsync(string venueId);
        Task InsertAsync(List<Venue> venues);

        Task<string?> GetVenueMongoIdByNameAsync(string venueName);

    }
}
