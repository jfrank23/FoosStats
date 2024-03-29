﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoosStats.Core;
using FoosStats.Core.Retrievers;
using FoosStats.Core.Updaters;
using FoosStats.Core.Creators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FoosStats.Pages.Players
{
    public class EditPlayerModel : PageModel
    {
        public IPlayerRetriever playerRetriever;
        public IPlayerUpdater playerUpdater;
        public ICreator<Player> playerCreator;
        private readonly IWebHostEnvironment env;

        [BindProperty]
        public Player Player { get; set; }

        public EditPlayerModel(IPlayerRetriever playerRetriever, IPlayerUpdater playerUpdater, ICreator<Player> playerCreator, IWebHostEnvironment env)
        {
            this.playerRetriever = playerRetriever;
            this.playerUpdater = playerUpdater;
            this.playerCreator = playerCreator;
            this.env = env;
        }

        public IActionResult OnGet(Guid? playerID)
        {
            if (!env.IsDevelopment())
            {
                return Redirect("../NoPermission");
            }
            if (playerID.HasValue)
            {
                Player = playerRetriever.GetPlayerById(playerID.Value);
            }
            else
            {
                Player = new Player();
            }

            if(Player == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if(!Guid.Equals(Player.ID,Guid.Empty))
            {
                playerUpdater.Update(Player);
            }
            else
            {
                playerCreator.Create(Player);
            }
            TempData["Message"] = "Player saved!";
            return RedirectToPage("./List");
        }
    }
}