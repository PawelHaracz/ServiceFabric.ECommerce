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
                var products = await _reliableStateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(tx, COLLECTION_NAME).ConfigureAwait(false);
                var allProducts = await products.CreateEnumerableAsync(tx, EnumerationMode.Unordered).ConfigureAwait(false);
                using (var enumerator = allProducts.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(ct).ConfigureAwait(false))
                    {
                        var current = enumerator.Current;
                        @result.Add(current.Value);
                    }
                }
            }            
            return @result;
        }


        public async Task<Product> GetProduct(Guid productId)
        {
            var @result = new List<Product>();
            using (var tx = _reliableStateManager.CreateTransaction())
            {
                var products = await _reliableStateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(tx, COLLECTION_NAME).ConfigureAwait(false);
                var isGotProduct = await products.TryGetValueAsync(tx, productId).ConfigureAwait(false);
                if (isGotProduct.HasValue)
                {
                    return isGotProduct.Value;
                }
            }

            throw new Exception($"Product doesn't exist {productId}");
        }

        public async Task Add(Product product)
        {
            var products = await _reliableStateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(COLLECTION_NAME);
            using (var tx = _reliableStateManager.CreateTransaction())
            {
               
                await products.AddOrUpdateAsync(tx, product.Id, product, (id, value) => product).ConfigureAwait(false);
                await tx.CommitAsync().ConfigureAwait(false);
            }
        }
    }
}