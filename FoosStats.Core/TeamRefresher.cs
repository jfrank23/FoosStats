using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core
{
    public interface ITeamRefresher
    {
        void Refresh();
    }
    public class TeamRefresher : ITeamRefresher
    {
        private readonly ITeamRepository teamRepository;

        public TeamRefresher(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }
        public void Refresh()
        {
            teamRepository.Refresh();
        }
    }
}
