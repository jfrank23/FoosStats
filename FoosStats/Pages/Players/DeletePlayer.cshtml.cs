﻿using System;
using FoosStats.Core;
using FoosStats.Core.Deleters;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages.Players
{
    public class DeletePlayerModel : PageModel
    {
        public IPlayerRetriever playerRetriever;
        private readonly ITeamRefresher teamRefresher;
        public IDeleter<Player> playerDeleter;
        private readonly IDeleter<Team> teamDeleter;
        public Player player;
        public DeletePlayerModel(IPlayerRetriever playerRetriever, ITeamRefresher teamRefresher, IDeleter<Team> teamDeleter)
        {
            this.playerRetriever = playerRetriever;
            this.teamRefresher = teamRefresher;
            this.teamDeleter = teamDeleter;
        }

        public IActionResult OnGet(Guid playerID)
        {
            if (playerID == Guid.Empty)
            {
                return Redirect("./NotFound");
            }
            player = playerRetriever.GetPlayerById(playerID);
            return Page();

        }
        public IActionResult OnPost(Guid playerID)
        {
            playerDeleter.Delete(playerID);
            teamRefresher.Refresh();
            return Redirect("~/Players/List");
        }
    }
}
