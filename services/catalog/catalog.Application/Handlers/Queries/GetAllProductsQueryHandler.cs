using AutoMapper;
using catalog.Application.Queries;
using catalog.Application.Responses;
using catalog.Core.Repositories;
using catalog.Core.Specs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Application.Handlers.Queries
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductQuery, Pagination<ProductResponeDto>>
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public GetAllProductsQueryHandler(IProductRepository productRepository , IMapper mapper)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<Pagination<ProductResponeDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllProducts(request.CatalogSpecsParams);
            var productResponse = _mapper.Map<Pagination<ProductResponeDto>>(products);
            return productResponse;
        }
    }
}
