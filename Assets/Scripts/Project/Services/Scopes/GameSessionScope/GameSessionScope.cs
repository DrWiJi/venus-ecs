using UnityEngine;
using VContainer.Unity;
using VContainer;
using Project.Services.VenusNetService;

namespace Project.Services.Scopes.GameSessionScope
{
    public class GameSessionScope : LifetimeScope
    {
        public GameBootstrap GameBootstrap;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<Project.Services.VenusNetService.VenusNetService>(Lifetime.Scoped);
            builder.RegisterInstance(GameBootstrap);
        }
    }
}