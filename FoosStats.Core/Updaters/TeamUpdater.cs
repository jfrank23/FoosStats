namespace FoosStats.Core.Updaters
{
    public class TeamUpdater : ITeamUpdater
    {
        private readonly ITeamRepository teamRepository;

        public TeamUpdater(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }
        public void Update(Game newGame)
        {
            teamRepository.Update(newGame);
        }
    }

    public interface ITeamUpdater
    {
        void Update(Game newGame);
    }
}
