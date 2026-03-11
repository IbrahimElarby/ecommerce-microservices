using AutoMapper;
using catalog.Application.Responses;
using catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Application.Queries
{
    public class GetAllProductsByIdQuery : IRequest<ProductResponeDto>
    {
     
        public string Id { get; set; }

        public GetAllProductsByIdQuery(string id )
        {
            Id = id;
        }

    }
}
