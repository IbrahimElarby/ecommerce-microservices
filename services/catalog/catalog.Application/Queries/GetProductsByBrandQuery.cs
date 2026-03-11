using catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Application.Queries
{
    public class GetProductsByBrandQuery : IRequest<List<ProductResponeDto>>
    {
        public string Brand { get; set; }
        public GetProductsByBrandQuery(string brand)
        {
            Brand = brand;
        }
    }
}
