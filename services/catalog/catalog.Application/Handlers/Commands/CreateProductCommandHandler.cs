using AutoMapper;
using catalog.Application.Commands;
using catalog.Application.Responses;
using catalog.Core.Entities;
using catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Application.Handlers.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponeDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository productRepository , IMapper mapper )
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<ProductResponeDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<Product>(request);
            var createdProduct = await  _productRepository.CreateProduct(productEntity);
            var productResponse = _mapper.Map<ProductResponeDto>(createdProduct);
            return productResponse;
        }
    }
}
