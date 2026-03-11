using catalog.Core.Entities;
using catalog.Core.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Pagination<Product>> GetAllProducts(CatalogSpecsParams catalogSpecsParams);

        Task<Product> GetProductById(string id);

        Task<Product> GetProductByName(string name);

        Task<IEnumerable<Product>> GetAllProductsByName(string typeName);

        Task<IEnumerable<Product>> GetProductsByBrand(string brandName);

        Task<Product> CreateProduct(Product product);

        Task<bool> UpdateProduct(Product product);

        Task<bool> DeleteProduct(string id);




    }
}
