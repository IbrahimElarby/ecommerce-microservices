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
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, List<TypesResponseDto>>
    {
        private readonly IProductTypeRepository _typeRepository;
        private readonly IMapper _mapper;

        public GetAllTypesQueryHandler(IMapper mapper , IProductTypeRepository productTypeRepository)
        {
            _mapper = mapper;
            _typeRepository = productTypeRepository;
        }

        public async Task<List<TypesResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _typeRepository.GetAllTypes();
            var typeResponse = _mapper.Map<List<TypesResponseDto>>(types.ToList());
            return typeResponse;
        }
    }
}
