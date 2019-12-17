using FoosStats.Core.Repositories;
using FoosStats.Core.Updaters;
using System.Linq;

namespace FoosStats.Core.Creators
{
    public class GameCreator :ICreator<Game>
    {
        private IGameRepository gameRepository;
        private readonly IPlayerUpdater playerUpdater;
        private readonly ITeamUpdater teamUpdater;

        public GameCreator(IGameRepository gameRepository, IPlayerUpdater playerUpdater, ITeamUpdater teamUpdater)
        {
            this.gameRepository = gameRepository;
            this.playerUpdater = playerUpdater;
            this.teamUpdater = teamUpdater;
        }
        public Game Create(Game game)
        {
            playerUpdater.UpdatePlayerAfterAddGame(game);
            var newGame = gameRepository.Add(game);
            var mostRecentGame = gameRepository.GetGames().OrderByDescending(g => g.GameTime).FirstOrDefault();
            if (newGame.GameID == mostRecentGame.GameID)
            {
                teamUpdater.Update(game);
            }
            else
            {
                teamUpdater.Refresh();
            }
            
            return newGame;
        }
    }
}
