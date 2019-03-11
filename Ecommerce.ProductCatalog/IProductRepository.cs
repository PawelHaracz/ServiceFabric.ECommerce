using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.ProductCatalog.Model;

namespace Ecommerce.ProductCatalog
{
    public interface IProductRepository  
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task Add(Product product);
    }
}