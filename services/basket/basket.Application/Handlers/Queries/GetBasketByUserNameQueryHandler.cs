using AutoMapper;
using basket.Application.Queries;
using basket.Application.Responses;
using basket.Core.Entites;
using basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basket.Application.Handlers.Queries
{
    public class GetBasketByUserNameQueryHandler : IRequestHandler<GetBasketByUserNameQuery, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;

        private readonly IMapper _mapper;
        public GetBasketByUserNameQueryHandler(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }



        public async Task<ShoppingCartResponse> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
        {
                var basket =  await _basketRepository.GetBasket(request.UserName);
                var basketResponse = _mapper.Map<ShoppingCartResponse>(basket);
                return basketResponse;
        }
    }
}
