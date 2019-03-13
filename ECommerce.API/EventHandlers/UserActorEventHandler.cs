using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserActor.Interfaces;

namespace ECommerce.API.EventHandlers
{
    public class UserActorEventHandler : IUserActorEvent
    {
        public void BasketUpdated(Guid productId, int quantity)
        {
            Console.WriteLine($"{productId} - {quantity}");
        }
    }
}
