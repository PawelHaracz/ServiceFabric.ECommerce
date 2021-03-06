﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.EventHandlers;
using ECommerce.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using UserActor.Interfaces;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        [HttpGet("{userId}")]
        public async Task<ApiBasket> Get(string userId)
        {
            var actor = GetActor(userId);
            var products = await actor.GetBasket();
            return new ApiBasket()
            {
                UserId =  userId,
                Items = products.Select(p => new ApiBasketItem
                {
                    ProductId = $"{p.Key}",
                    Quantity = p.Value
                }).ToArray()
            };
        }

        [HttpPost("{userId}")]
        public async Task Add(string userId, [FromBody]ApiBasketAddRequest request)
        {
            var actor = GetActor(userId);
            await actor.SubscribeAsync<IUserActorEvent>(new UserActorEventHandler()).ConfigureAwait(false);
            await actor.AddToBasket(request.ProductId, request.Quantity).ConfigureAwait(false);
        }

        [HttpDelete("{userId}")]
        public async Task Delete(string userId)
        {
            var actor = GetActor(userId);
            await actor.ClearBasket().ConfigureAwait(false);
        }

        private IUserActor GetActor(string userId)
        {
            return ActorProxy.Create<IUserActor>(new ActorId(userId), new Uri("fabric:/ECommerce/UserActorService"));
        }
    }
}