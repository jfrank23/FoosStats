using FoosStats.Core.Repositories;
using System;

namespace FoosStats.Core.Deleters
{
    public class TeamDeleter : IDeleter<Team>
    {
        private readonly ITeamRepository teamRepository;

        public TeamDeleter(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }
        public void Delete(Guid playerID)
        {
            teamRepository.Delete(playerID);
        }
    }
}
