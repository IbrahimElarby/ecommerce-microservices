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
    public class GetAllBrandQueryHandler : IRequestHandler<GetAllBrandsQuery, List<BrandResponseDto>>
    {

        private readonly IProductBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        public GetAllBrandQueryHandler(IMapper mapper , IProductBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        public async Task<List<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await  _brandRepository.GetAllBrands();
            var brandResponse = _mapper.Map<List<BrandResponseDto>>(brands.ToList());
            return brandResponse;

        }
    }
}
