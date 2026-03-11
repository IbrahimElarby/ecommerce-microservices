using MediatR;
using ordering.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordering.Application.Queries
{
    public class GetOrderListQuery : IRequest<List<OrderResponse>>
    {
        public string? UserName { get; set; }
        public GetOrderListQuery(string userName)
        {
            UserName = userName;
        }

    }
}
