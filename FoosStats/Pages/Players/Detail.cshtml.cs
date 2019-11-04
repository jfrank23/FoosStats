﻿using System;
using System.Collections.Generic;
using FoosStats.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using FoosStats.Core.Retrievers;

namespace FoosStats.Pages.Players
{
    public class DetailModel : PageModel
    {
        public IPlayerRetriever playerRetriever;
        public IGameRetriever gameRetriever;
        public IPlayerDetailHandler detailHandler;
        public IEnumerable<DisplayGame> top3Games { get; set; }
        public string MostPlayedWith { get; set; }
        public string BestPercentageWith { get; set; }
        public float GoalsForPerGame{ get; set; }
        public float GoalsAgainstPerGame{ get; set; }
        public float WinPercentage{ get; set; }
        public int WinPctStanding { get; set; }
        public int GoalsForAvgStanding { get; set; }
        public int GoalsAgainstAvgStanding { get; set; }
        public Player Player { get; private set; }



        public DetailModel(IPlayerRetriever playerRetriever, IGameRetriever gameRetriever, IPlayerDetailHandler detailHandler)
        {
            this.playerRetriever = playerRetriever;
            this.gameRetriever = gameRetriever;
            this.detailHandler = detailHandler;
        }

        public IActionResult OnGet(Guid playerID)
        {
            Player = playerRetriever.GetPlayerById(playerID);
            if (Player== null)
            {
                return RedirectToPage("./NotFound");
            }
            detailHandler.SetupTeammateStats(playerID);
            top3Games = detailHandler.GetTop3Games(playerID);
            MostPlayedWith = detailHandler.MostPlayedWith();
            BestPercentageWith = detailHandler.BestWinPercentageTeammate();
            WinPercentage = (float)Player.GamesWon / Player.GamesPlayed *100;
            GoalsAgainstPerGame = (float)Player.GoalsAgainst / Player.GamesPlayed;
            GoalsForPerGame = (float)Player.GoalsFor / Player.GamesPlayed;
            WinPctStanding = detailHandler.WinPercentageStanding(Player);
            GoalsForAvgStanding = detailHandler.GoalsForAverageStanding(Player);
            GoalsAgainstAvgStanding = detailHandler.GoalsAgainstAverageStanding(Player);
            return Page();
        }

        
   
    }
}