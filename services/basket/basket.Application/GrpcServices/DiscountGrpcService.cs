using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basket.Application.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountGrpcService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountGrpcService)
        {
            _discountGrpcService = discountGrpcService;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var GrpcDiscountRequest = new GetDiscountRequest { ProductName = productName };
            return await _discountGrpcService.GetDiscountAsync(GrpcDiscountRequest);
        }
    }
}
