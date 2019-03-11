using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.ProductCatalog.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Ecommerce.ProductCatalog
{
    public class ServiceFabricProductRepository : IProductRepository
    {
        private const string COLLECTION_NAME = "Products";
        private readonly IReliableStateManager _reliableStateManager;

        public ServiceFabricProductRepository(IReliableStateManager reliableStateManager)
        {
            _reliableStateManager = reliableStateManager;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var ct = new CancellationToken();
            var @result = new List<Product>();
            using (var tx = _reliableStateManager.CreateTransaction())
            {
                var products = await _reliableStateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(tx, COLLECTION_NAME);
                var allProducts = await products.CreateEnumerableAsync(tx, EnumerationMode.Unordered);
                using (var enumerator = allProducts.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(ct))
                    {
                        var current = enumerator.Current;
                        @result.Add(current.Value);
                    }
                }
            }

            return @result;
        }

        public async Task Add(Product product)
        {
            var products = await _reliableStateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(COLLECTION_NAME);
            using (var tx = _reliableStateManager.CreateTransaction())
            {
               
                await products.AddOrUpdateAsync(tx, product.Id, product, (id, value) => product);
                await tx.CommitAsync();
            }
        }
    }
}