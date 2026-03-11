using catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Infrastructure.Data.Contexts
{
    public static class CatalogContextSeed
    {
        public static async Task SeedData (IMongoCollection<Product> productCollection)
        {
       
            var existProduct = productCollection.Find(p => true).Any();
            if (existProduct) 
                return;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedData", "products.json");
            if (!File.Exists(filePath))
                return;
            var productData = await File.ReadAllTextAsync(filePath);
            var products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(productData);
            if (products != null && products.Any())
            {
                await productCollection.InsertManyAsync(products);
            }
        }
    }
}
