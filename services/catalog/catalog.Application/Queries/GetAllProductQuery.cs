using catalog.Application.Responses;
using catalog.Core.Specs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Application.Queries
{
    public class GetAllProductQuery : IRequest<Pagination<ProductResponeDto>>
    {
       public CatalogSpecsParams CatalogSpecsParams { get; set; }

        public GetAllProductQuery(CatalogSpecsParams catalogSpecsParams)
        {
            CatalogSpecsParams = catalogSpecsParams;
        }
    }
}
