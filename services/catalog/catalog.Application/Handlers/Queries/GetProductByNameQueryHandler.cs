using AutoMapper;
using catalog.Application.Queries;
using catalog.Application.Responses;
using catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Application.Handlers.Queries
{
    public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, ProductResponeDto>
    { 
        private readonly IMapper _mapper;

        private readonly IProductRepository _productRepository;

        public GetProductByNameQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<ProductResponeDto> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var product =  await _productRepository.GetProductByName(request.Name);
            var productResponse = _mapper.Map<ProductResponeDto>(product);  
            return productResponse;

        }
    }
}
