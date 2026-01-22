using Goal_Project.Repository.Interface;

namespace Goal_Project.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IVenueRepo VenueRepo { get; }
        public ITeamRepo TeamRepo { get; }
        public IPlayerRepo PlayerRepo { get; }
        public IMatchRepo MatchRepo { get; }

        public IGoalRepo GoalRepo { get; }
    }
}
