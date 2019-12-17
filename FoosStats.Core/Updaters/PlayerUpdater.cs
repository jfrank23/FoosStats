using FoosStats.Core.Repositories;

namespace FoosStats.Core.Updaters
{
    public class PlayerUpdater : IPlayerUpdater
    {
        private IPlayerRepository playerRepository;
        public PlayerUpdater(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }
        public Player Update(Player player)
        {
            return playerRepository.Update(player);
        }
        public void UpdatePlayerAfterAddGame(Game newGame)
        {
            foreach (var id in new[] { newGame.BlueDefense, newGame.BlueOffense, newGame.RedDefense, newGame.RedOffense })
            {
                var player = playerRepository.GetPlayerById(id);
                if (player.ID == newGame.BlueDefense || player.ID == newGame.BlueOffense)
                {
                    player.GoalsFor += newGame.BlueScore;
                    player.GoalsAgainst += newGame.RedScore;
                    player.GamesPlayed += 1;
                    if (newGame.BlueScore == 10)
                    {
                        player.GamesWon += 1;

                    }
                    else
                    {
                        player.GamesLost += 1;
                    }
                }
                if (player.ID == newGame.RedDefense || player.ID == newGame.RedOffense)
                {
                    player.GoalsFor += newGame.RedScore;
                    player.GoalsAgainst += newGame.BlueScore;
                    player.GamesPlayed += 1;
                    if (newGame.RedScore == 10)
                    {
                        player.GamesWon += 1;

                    }
                    else
                    {
                        player.GamesLost += 1;
                    }
                }
                Update(player);
            }
        }
        public void UpdatePlayerAfterDeleteGame(Game oldGame)
        {
            foreach (var id in new[] { oldGame.BlueDefense, oldGame.BlueOffense, oldGame.RedDefense, oldGame.RedOffense })
            {
                var player = playerRepository.GetPlayerById(id);

                if (player.ID == oldGame.BlueDefense || player.ID == oldGame.BlueOffense)
                {
                    player.GoalsFor -= oldGame.BlueScore;
                    player.GoalsAgainst -= oldGame.RedScore;
                    player.GamesPlayed -= 1;
                    if (oldGame.BlueScore == 10)
                    {
                        player.GamesWon -= 1;

                    }
                    else
                    {
                        player.GamesLost -= 1;
                    }
                }
                if (player.ID == oldGame.RedDefense || player.ID == oldGame.RedOffense)
                {
                    player.GoalsFor -= oldGame.RedScore;
                    player.GoalsAgainst -= oldGame.BlueScore;
                    player.GamesPlayed -= 1;
                    if (oldGame.RedScore == 10)
                    {
                        player.GamesWon -= 1;

                    }
                    else
                    {
                        player.GamesLost -= 1;
                    }
                }
                Update(player);
            }
        }
    }

    public interface IPlayerUpdater
    {
        Player Update(Player player);
        void UpdatePlayerAfterAddGame(Game newGame);
        void UpdatePlayerAfterDeleteGame(Game oldGame);
    }
}
