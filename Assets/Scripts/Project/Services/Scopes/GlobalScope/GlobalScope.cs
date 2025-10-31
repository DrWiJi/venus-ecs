using UnityEngine;
using Project.Services.NetTransportService;
using VContainer.Unity;
using VContainer;

namespace Project.Services.Scopes.GlobalScope
{
    public class GlobalScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<NetTransport>(Lifetime.Scoped).As<INetTransport>();
        }
    }
}
