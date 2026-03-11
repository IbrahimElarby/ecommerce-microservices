using AutoMapper;
using EventBus.Messages.Event;
using MassTransit;
using MediatR;
using ordering.Application.Commands;

namespace ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumerV2: IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMediator _mediator;

        private readonly IMapper _mapper;
        public BasketOrderingConsumerV2(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            var command = _mapper.Map<CheckoutOrderCommandV2>(context.Message);
            var result = await _mediator.Send(command);

        }
    }
}
