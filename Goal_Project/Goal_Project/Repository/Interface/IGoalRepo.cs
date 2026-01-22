using Goal_Project.Models;

namespace Goal_Project.Repository.Interface
{
    public interface IGoalRepo
    {
        Task<List<Goal>> GetAllAsync();
        Task InsertAsync(Goal goal);
        Task<List<Goal>> GetGoalsByMatchGuidAsync(Guid matchGuid);
    }
}
