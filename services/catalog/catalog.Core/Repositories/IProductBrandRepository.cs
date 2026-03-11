using catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Core.Repositories
{
    public interface IProductBrandRepository
    {
        Task<IEnumerable<ProductBrand>> GetAllBrands();
    }
}
