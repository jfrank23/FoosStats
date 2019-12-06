using System;
using System.Collections.Generic;

namespace FoosStats.Core.Repositories
{
    public interface ITeamRepository
    {
        void Clear();
        Team Create(Team newTeam);
        void Delete(Guid playerID);
        DisplayTeam GetTeamById(Guid TeamID);
        DisplayTeam GetTeamByPlayers(Guid DefenseID, Guid OffenseID);
        IEnumerable<DisplayTeam> GetTeams();
        void Update(Team updatedTeam);
    }
}
