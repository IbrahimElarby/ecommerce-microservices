using basket.Core.Entites;
using basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDistributedCache _redisCache;
        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);

            if(string.IsNullOrEmpty(basket))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

      
           public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(2)
            };

            await _redisCache.SetStringAsync(
                cart.UserName,
                JsonConvert.SerializeObject(cart),
                options
            );

            return cart;
        
        }
        public async Task DeleteBasket(string userName)
        {
            var basket  = await _redisCache.GetStringAsync(userName);
            if(basket != null)
            {
                await _redisCache.RemoveAsync(userName);
            }
        }
    }
}
