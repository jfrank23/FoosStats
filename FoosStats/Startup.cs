using FoosStats.Core;
using FoosStats.Core.Creators;
using FoosStats.Core.Deleters;
using FoosStats.Core.PageSpecific;
using FoosStats.Core.Repositories;
using FoosStats.Core.Retrievers;
using FoosStats.Core.Updaters;
using FoosStats.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoosStats
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSingleton<IPlayerRepository, LitePlayerRepository>();
            services.AddSingleton<IGameRepository, LiteGameRepository>();
            services.AddSingleton<ITeamRepository, LiteTeamRepository>();
            services.AddSingleton<IHistoricalData, InMemoryHistoricalData>();

            services.AddScoped<ILeaderboards, InMemoryLeaderboards>();
            services.AddScoped<ITeamGenerator, TeamGenerator>();

            services.AddScoped<ICreator<Game>, GameCreator>();
            services.AddScoped<ICreator<Player>, PlayerCreator>();
            services.AddScoped<ICreator<Team>,TeamCreator>();

            services.AddScoped<IDeleter<Game>, GameDeleter>();
            services.AddScoped<IDeleter<Player>, PlayerDeleter>();
            services.AddScoped<IDeleter<Team>, TeamDeleter>();

            services.AddScoped<IGameRetriever, GameRetriever>();
            services.AddScoped<IPlayerRetriever, PlayerRetriever>();
            services.AddScoped<ITeamRetriever, TeamRetriever>();

            services.AddScoped<IPlayerDetailRetriever, PlayerDetailRetriever>();
            services.AddScoped<IHomePageStatRetriever, HomePageStatRetriever>();
            services.AddScoped<TeamDetailRetriever, TeamDetailRetriever>();

            services.AddScoped<IUpdater<Game>, GameUpdater>();
            services.AddScoped<IPlayerUpdater, PlayerUpdater>();
            services.AddScoped<ITeamUpdater, TeamUpdater>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
