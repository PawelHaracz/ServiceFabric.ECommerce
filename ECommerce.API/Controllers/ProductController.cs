using ECommerce.API.Model;
using ECommerce.ProductCatalog.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductCatalogService _catalogService;
        public ProductController()
        {        
            _catalogService = ServiceProxy.Create<IProductCatalogService>(new Uri("fabric:/ECommerce/Ecommerce.ProductCatalog"), new ServicePartitionKey(0L));
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> Get()
        {
            var products = await _catalogService.GetAllProducts().ConfigureAwait(false);
            return products.Select(p => new ApiProduct
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                IsAvailable = p.Availability > 0,
                Price = p.Price
            });
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] ApiProduct product)
        {
            var mappedProduct = new Product()
            {
                Name = product.Name,
                Id = product.Id,
                Description = product.Description,
                Price = product.Price,
                Availability = 100
            };
            await _catalogService.AddProduct(mappedProduct).ConfigureAwait(false);
        }
    }
}
