namespace FoosStats.Core.Creators
{
    public class TeamCreator : ICreator<Team>
    {
        private readonly ITeamRepository teamRepository;

        public TeamCreator(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }
        public Team Create(Team team)
        {
            return teamRepository.Add(team);
        }
    }

}
