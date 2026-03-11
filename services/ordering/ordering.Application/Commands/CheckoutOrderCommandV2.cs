using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordering.Application.Commands
{
    public class CheckoutOrderCommandV2 : IRequest<int>
    {
    public string UserName { get; set; }
    public decimal TotalPrice { get; set; }
    }
}
