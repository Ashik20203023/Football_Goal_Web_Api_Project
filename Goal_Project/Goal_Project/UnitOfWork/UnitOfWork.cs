using Goal_Project.Repository.Interface;

namespace Goal_Project.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        public IVenueRepo VenueRepo { get; }
        public ITeamRepo TeamRepo { get; }
        public IPlayerRepo PlayerRepo { get; }
        public IMatchRepo MatchRepo { get; }

        public IGoalRepo GoalRepo { get; }

        public UnitOfWork(IVenueRepo venueRepo, ITeamRepo teamRepo, IPlayerRepo playerRepo, IMatchRepo matchRepo, IGoalRepo goalRepo)
        {
            VenueRepo = venueRepo;
            TeamRepo = teamRepo;
            PlayerRepo = playerRepo;
            MatchRepo = matchRepo;
            GoalRepo = goalRepo;
        }

        //private readonly IServiceProvider _serviceProvider;

        //private IVenueRepo? _venueRepo;
        //private ITeamRepo? _teamRepo;
        //private IPlayerRepo? _playerRepo;
        //private IMatchRepo? _matchRepo;
        //private IGoalRepo? _goalRepo;

        //public UnitOfWork(IServiceProvider serviceProvider)
        //{
        //    _serviceProvider = serviceProvider;
        //}

        //public IVenueRepo VenueRepo =>
        //    _venueRepo ??= _serviceProvider.GetRequiredService<IVenueRepo>();

        //public ITeamRepo TeamRepo =>
        //    _teamRepo ??= _serviceProvider.GetRequiredService<ITeamRepo>();

        //public IPlayerRepo PlayerRepo =>
        //    _playerRepo ??= _serviceProvider.GetRequiredService<IPlayerRepo>();

        //public IMatchRepo MatchRepo =>
        //    _matchRepo ??= _serviceProvider.GetRequiredService<IMatchRepo>();

        //public IGoalRepo GoalRepo =>
        //    _goalRepo ??= _serviceProvider.GetRequiredService<IGoalRepo>();



    }
}
