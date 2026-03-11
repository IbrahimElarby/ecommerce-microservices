using FluentValidation;
using ordering.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordering.Application.Validators
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator() 
        {
            RuleFor(o => o.Id).NotEmpty().WithMessage("Id is required.").GreaterThan(0).WithMessage("Id must be greater than zero.");
            RuleFor(o => o.UserName).NotEmpty().WithMessage("UserName is required.");
            RuleFor(o => o.EmailAddress).NotEmpty().WithMessage("EmailAddress is required.").EmailAddress().WithMessage("Invalid email format.");
            RuleFor(o => o.FirstName).NotEmpty().WithMessage("FirstName is required.");
            RuleFor(o => o.LastName).NotEmpty().WithMessage("LastName is required.");
            RuleFor(o => o.AddressLine).NotEmpty().WithMessage("AddressLine is required.");
            RuleFor(o=>o.TotalPrice).NotEmpty().WithMessage("TotalPrice is required.").GreaterThan(-1).WithMessage("TotalPrice must be greater than or equal zero.");
        }
    }
}
