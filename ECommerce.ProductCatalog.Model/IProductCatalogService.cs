﻿using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: FabricTransportServiceRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2, RemotingClientVersion = RemotingClientVersion.V2)]
namespace ECommerce.ProductCatalog.Model
{
    public interface IProductCatalogService : IService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task AddProduct(Product product);
    }
}