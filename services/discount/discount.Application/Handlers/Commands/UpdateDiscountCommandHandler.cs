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
    public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand,CouponModel>
    {
        private readonly IMapper _mapper;

        private readonly ICouponRepository _repository;
        public UpdateDiscountCommandHandler(IMapper mapper, ICouponRepository repository = null)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = _mapper.Map<Core.Entities.Coupon>(request);
            await _repository.UpdateDiscount(coupon);
            return _mapper.Map<CouponModel>(coupon);
        }
    }
}
