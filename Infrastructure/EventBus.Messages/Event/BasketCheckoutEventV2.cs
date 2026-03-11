using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Event
{
    public class BasketCheckoutEventV2
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
