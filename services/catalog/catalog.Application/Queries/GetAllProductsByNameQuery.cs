using catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Application.Queries
{
    public class GetAllProductsByNameQuery : IRequest<List<ProductResponeDto>>
    {
        public string Name { get; set; }
        public GetAllProductsByNameQuery(string name)
        {
            Name = name;
        }
    }
}
