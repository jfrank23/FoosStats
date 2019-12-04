using System;
using System.Collections.Generic;

namespace FoosStats.Core
{
    public interface ITeamRepository
    {
        void Clear();
        Team Create(Team newTeam);
        void Delete(Guid playerID);
        DisplayTeam GetTeamByPlayers(Guid DefenseID, Guid OffenseID);
        IEnumerable<DisplayTeam> GetTeams();
        void Update(Team updatedTeam);
    }
}
