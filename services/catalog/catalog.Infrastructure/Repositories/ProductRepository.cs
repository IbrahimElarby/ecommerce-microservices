using catalog.Core.Entities;
using catalog.Core.Repositories;
using catalog.Core.Specs;
using catalog.Infrastructure.Data.Contexts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IProductTypeRepository, IProductBrandRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext catalogContext)
        {
            _context = catalogContext;
        }
        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _context.Brands.Find(p => true).ToListAsync();
        }

        public async Task<Pagination<Product>> GetAllProducts(CatalogSpecsParams catalogSpecsParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;

            if(!string.IsNullOrEmpty(catalogSpecsParams.Search))
            {
                filter = filter & builder.Where(p=>p.Name.ToLower().Contains(catalogSpecsParams.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(catalogSpecsParams.BrandId))
            {
                var brandFilter = builder.Eq(p => p.Brand.Id, catalogSpecsParams.BrandId);
                filter = filter & brandFilter;
            }
            if (!string.IsNullOrEmpty(catalogSpecsParams.TypeId))
            {
                var typeFilter = builder.Eq(p => p.Type.Id, catalogSpecsParams.TypeId);
                filter = filter & typeFilter;
            }
            var totalitems = await _context.Products.CountDocumentsAsync(filter);
            var data = await DataFilter(catalogSpecsParams, filter);  

            return new Pagination<Product>(catalogSpecsParams.PageIndex,catalogSpecsParams.PageSize,(int)totalitems,data);
        }

        public async Task<IEnumerable<Product>> GetAllProductsByName(string typeName)
        {
            return await _context.Products.Find(p => p.Name == typeName).ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByName(string name)
        {
            return await _context.Products.Find(p => p.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByBrand(string brandName)
        {
            return await _context.Products.Find(p => p.Brand.Name == brandName).ToListAsync();
        }

        async Task<IEnumerable<ProductType>> IProductTypeRepository.GetAllTypes()
        {
            return await _context.Types.Find(p => true).ToListAsync();
        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var deletedProduct = await _context.Products.DeleteOneAsync(p => p.Id == id);
            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updatedProduct = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
        }

        private async Task<IReadOnlyList<Product>> DataFilter (CatalogSpecsParams catalogSpecsParams , FilterDefinition<Product> filter)
        {
            var sortDef = Builders<Product>.Sort.Ascending("Name");
            if (!string.IsNullOrEmpty(catalogSpecsParams.Sort))
            {
                switch (catalogSpecsParams.Sort)
                {
                    case "priceAsc":
                        sortDef = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;

                    case "priceDesc":
                        sortDef = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sortDef = Builders<Product>.Sort.Ascending("Name");
                        break;
                }
            }
                return await _context
                    .Products
                    .Find(filter)
                    .Sort(sortDef)
                    .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
                    .Limit(catalogSpecsParams.PageSize)
                    .ToListAsync();
            }
        }
    }

