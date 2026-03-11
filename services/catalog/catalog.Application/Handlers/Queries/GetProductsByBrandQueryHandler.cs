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
    public class GetProductsByBrandQueryHandler : IRequestHandler<GetProductsByBrandQuery, List<ProductResponeDto>>
    {
        private readonly IMapper _mapper;   

        private readonly IProductRepository _productRepository;

        public GetProductsByBrandQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<List<ProductResponeDto>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
        {
           var products =  await _productRepository.GetProductsByBrand(request.Brand);
              var productResponse = _mapper.Map<List<ProductResponeDto>>(products.ToList());
                return productResponse;
        }
    }
}
