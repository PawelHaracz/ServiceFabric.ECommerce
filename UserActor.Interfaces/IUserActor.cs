using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace UserActor.Interfaces
{
    public interface IUserActor : IActor, IActorEventPublisher<IUserActorEvent>
    {
        Task AddToBasket(Guid productId, int quantity);

        Task<IDictionary<Guid, int>> GetBasket();

        Task ClearBasket();
    }
}
