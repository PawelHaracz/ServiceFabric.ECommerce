using System;
using Microsoft.ServiceFabric.Actors;

namespace UserActor.Interfaces
{
    public interface IUserActorEvent : IActorEvents
    {
        void BasketUpdated(Guid productId, int quantity);
    }
}