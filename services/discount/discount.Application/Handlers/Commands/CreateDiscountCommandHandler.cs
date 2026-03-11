using AutoMapper;
using discount.Application.Commands;
using discount.Core.Repository;
using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discount.Application.Handlers.Commands
{
    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
    {
        private readonly IMapper _mapper;
        private readonly ICouponRepository _repository;

        public CreateDiscountCommandHandler(IMapper mapper, ICouponRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon =  _mapper.Map<Core.Entities.Coupon>(request);
            await _repository.CreateDiscount(coupon);
           return _mapper.Map<CouponModel>(coupon);

        }
    }
}
