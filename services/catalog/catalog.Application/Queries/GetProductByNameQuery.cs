using catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Application.Queries
{
    public class GetProductByNameQuery : IRequest<ProductResponeDto>
    {
       public string Name { get; set; }
        public GetProductByNameQuery(string name)
        {
            Name = name;
        }
        
    }
}
