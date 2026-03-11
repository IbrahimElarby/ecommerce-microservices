using AutoMapper;
using discount.Application.Queries;
using discount.Core.Repository;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discount.Application.Handlers.Queries
{
    public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
    {

        private readonly IMapper _mapper;
        private readonly ICouponRepository _repository;
        private readonly ILogger<GetDiscountQueryHandler> _logger;
        public GetDiscountQueryHandler(IMapper mapper, ICouponRepository repository, ILogger<GetDiscountQueryHandler> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }
        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound,$"No Discount Available for this product {request.ProductName}"));
            }
            return _mapper.Map<CouponModel>(coupon);
        }

    }
}
