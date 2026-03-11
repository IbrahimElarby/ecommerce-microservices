using AutoMapper;
using EventBus.Messages.Event;
using MassTransit;
using MediatR;
using ordering.Application.Commands;

namespace ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;

        private readonly IMapper _mapper;
        public BasketOrderingConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediator.Send(command);

        }

        
    }
}
