using Goal_Project.Models;


namespace Goal_Project.Service.Interface
{
    public interface IVenueService
    {
        Task<string> AddVenueAsync(Venue venue);
        Task<List<Venue>> GetAllAsync();
        

    }
}
