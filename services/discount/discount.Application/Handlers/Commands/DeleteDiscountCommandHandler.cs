using discount.Application.Commands;
using discount.Core.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discount.Application.Handlers.Commands
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly ICouponRepository _repository;
        public DeleteDiscountCommandHandler(ICouponRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteDiscount(request.ProductName);
            return result;
        }
    }
}
