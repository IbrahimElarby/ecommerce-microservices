using catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Infrastructure.Data.Contexts
{
    public static class TypeContextSeed
    {
        public static async Task SeedData (IMongoCollection<ProductType> typeCollection)
        {
            var existType = typeCollection.Find(p => true).Any();
            if (existType) 
                return;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedData", "types.json");
            if (!File.Exists(filePath))
                return;
            var typeData = await File.ReadAllTextAsync(filePath);
            var types = System.Text.Json.JsonSerializer.Deserialize<List<ProductType>>(typeData);
            if (types != null && types.Any())
            {
                await typeCollection.InsertManyAsync(types);
            }
        }
    }
}
