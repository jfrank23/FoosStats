using FoosStats.Core.Retrievers;
using FoosStats.Core.Updaters;
using System.Linq;

namespace FoosStats.Core
{
    public interface ITeamRefresher
    {
        void Refresh();
    }
    public class TeamRefresher : ITeamRefresher
    {
        private readonly ITeamRepository teamRepository;
        private readonly IPlayerRetriever playerRetriever;
        private readonly IGameRetriever gameRetriever;
        private readonly ITeamUpdater teamUpdater;

        public TeamRefresher(ITeamRepository teamRepository, IPlayerRetriever playerRetriever, IGameRetriever gameRetriever,ITeamUpdater teamUpdater)
        {
            this.teamRepository = teamRepository;
            this.playerRetriever = playerRetriever;
            this.gameRetriever = gameRetriever;
            this.teamUpdater = teamUpdater;
        }
        public void Refresh()
        {
            teamRepository.Clear();
            foreach (var game in gameRetriever.GetAllGames().OrderBy(g => g.GameTime))
            {
                teamUpdater.Update(game);
            }
            
        }
    }
}
