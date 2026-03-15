using Asp.Versioning;
using AutoMapper;
using basket.Application.Commands;
using basket.Application.Queries;
using basket.Core.Entites;
using EventBus.Messages.Event;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace basket.API.Controllers.V2
{
    
    [ApiVersion("2.0")]
    
    public class BasketController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;
        public BasketController(IMediator mediator, IMapper mapper, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
        {
            //get existing basket with total price
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null)
            {
                return BadRequest();
            }
            var eventMessage = _mapper.Map<BasketCheckoutEventV2>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(eventMessage);

            _logger.LogInformation("BasketCheckoutEvent published for user {UserName} with total price {TotalPrice} with V2 EndPoint", basketCheckout.UserName, eventMessage.TotalPrice);
            //delete the basket
            var deleteCommand = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
            await _mediator.Send(deleteCommand);
            return Accepted();


        }

    }
}
