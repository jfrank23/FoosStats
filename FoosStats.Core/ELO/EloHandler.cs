﻿using System;

namespace FoosStats.Core.ELO
{
    /* Expected score player A = 1/(1+10^((Rating_B - Rating_A)/400)
     * Expected score player B = 1/(1+10^((Rating_A - Rating_B)/400)
     * Updated player A rank = Rating_A + K *(Actual Score A - Expected score A)
     * Updated player B rank = Rating_B + K*(Actual Score B - Expected Score B)
     * 
     * K-Factor for FIDE
     * K = 40, for a player new to the rating list until the completion of events with a total of 30 games and for all players until their 18th birthday, as long as their rating remains under 2300.
     * K = 20, for players with a rating always under 2400.
     * K = 10, for players with any published rating of at least 2400 and at least 30 games played in previous events. Thereafter it remains permanently at 10.
     * 
     * Starting Score = 1200
     */
    public static class EloHandler
    {
        public static readonly int StartingScore = 1200;
        public static int[] UpdatedRanks(Team blue, Team red, Game game)
        {
            var expectedScores = ExpectedScores(blue.Rank, red.Rank);
            var K_Blue = K_Decision(blue);
            var K_Red = K_Decision(red);
            var updatedRankBlue = (int)Math.Round(blue.Rank + K_Blue * (ActualScore(game)[0] - expectedScores[0]));
            var updatedRankRed = (int)Math.Round(red.Rank + K_Red * (ActualScore(game)[1] - expectedScores[1]));
            return new int[] { updatedRankBlue, updatedRankRed };
        }

        private static double[] ExpectedScores(int rankBlue, int rankRed)
        {
            var blueAdvantage = 100;
            rankBlue += blueAdvantage;
            var expectedScoreBlue = 1 / (1 + Math.Pow(10, (rankRed - rankBlue) / 400));
            var expectedScoreRed = 1 / (1 + Math.Pow(10, (rankBlue - rankRed) / 400));
            return new double[] { expectedScoreBlue, expectedScoreRed };
        }
        
        private static int K_Decision(Team team)
        {
            var multiplier = 5;
            if (team.GamesPlayed < 10)
            {
                return 40 * multiplier;
            }
            else if (team.Rank < 2400)
            {
                return 20 * multiplier;
            }
            else
            {
                return 10 * multiplier;
            }
        }
        private static int[] ActualScore(Game game)
        {
            if(game.BlueScore > game.RedScore)
            {
                return new int[] { 1, 0 };
            }
            else
            {
                return new int[] { 0, 1 };
            }
        }
    }
}
