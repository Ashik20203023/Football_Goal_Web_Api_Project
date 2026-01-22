
using Goal_Project.Models;
using Goal_Project.Repository.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Goal_Project.Repository.Implimentation
{
    public class VenueRepo:IVenueRepo
    {
        private readonly IMongoCollection<Venue> venueCollection;
        public VenueRepo(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            venueCollection = database.GetCollection<Venue>("Venues");
        }

        public Task<List<Venue>> GetAllAsync()
        {
            return venueCollection.Find(Builders<Venue>.Filter.Empty).ToListAsync();
        }

        public async Task<bool> VenueNameAsync(string venueName)
        {
            var count = await venueCollection.Find(v => v.VenueName == venueName).CountDocumentsAsync();
            return count > 0;

        }


        public async Task<bool> VenueIdAsync(string venueId)
        {
            var count = await venueCollection.Find(v => v.VenueId == venueId).CountDocumentsAsync();
            return count > 0;
        }

        public async Task InsertAsync(List<Venue> venues)
        {
            if (venues == null || venues.Count == 0)
                return;

            await venueCollection.InsertManyAsync(venues);
        }


        public async Task<string?> GetVenueMongoIdByNameAsync(string venueName)
        {
            var venue = await venueCollection.Find(v => v.VenueName == venueName).FirstOrDefaultAsync();
            return venue?.Id;
        }


    }
}
