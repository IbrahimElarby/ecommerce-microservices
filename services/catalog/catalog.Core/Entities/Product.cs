using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Core.Entities
{
    public class Product : BaseClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string summary { get; set; }

        public string ImageFile { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal Price { get; set; }

        public ProductType Type { get; set; }

        public ProductBrand Brand { get; set; }

    }
}
