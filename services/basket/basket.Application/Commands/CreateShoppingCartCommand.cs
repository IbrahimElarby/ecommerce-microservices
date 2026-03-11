using basket.Application.Responses;
using basket.Core.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basket.Application.Commands
{
    public class CreateShoppingCartCommand : IRequest<ShoppingCartResponse>
    {
        public string UserName { get; set; }

        public List<ShoppingCartItem> Items { get; set; } 
        public CreateShoppingCartCommand(string userName , List<ShoppingCartItem> items)
        {
            UserName = userName;
            Items =   items ;
        }
    }
}
