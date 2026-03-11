using catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Infrastructure.Data.Contexts
{
    public static class BrandContextSeed
    {
        public static async Task SeedData (IMongoCollection<ProductBrand> brandCollection)
        {
            var existBrand = brandCollection.Find(p => true).Any();
            if (existBrand) 
                return;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedData", "brands.json");
            if (!File.Exists(filePath))
                return;

            var brandData = await File.ReadAllTextAsync(filePath);
            var brands = System.Text.Json.JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

            if (brands != null && brands.Any())
            {
                await brandCollection.InsertManyAsync(brands);
            }
        }
    }
}
