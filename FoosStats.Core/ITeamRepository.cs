using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core
{
    public interface ITeamRepository
    {
        Team Add(Team newTeam);
        void Delete(Guid playerID);
        IEnumerable<DisplayTeam> GetTeams();
        void Refresh();
        void Update(Game newGame);
    }
}
