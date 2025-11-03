using UnityEngine;
using VContainer.Unity;
using VContainer;
using Project.Services.VenusNetService;
using Project.Core;

namespace Project.Services.Scopes.GameSessionScope
{
    public class GameSessionScope : LifetimeScope
    {
        public BaseGameBootstrap GameBootstrap;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<VenusNetService.VenusNetService>(Lifetime.Scoped);
            builder.RegisterInstance(GameBootstrap);
        }
    }
}