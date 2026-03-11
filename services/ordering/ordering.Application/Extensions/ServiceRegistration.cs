using EventBus.Messages.Event;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ordering.Application.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ordering.Application.Extensions
{
    public static class ServiceRegistration
    {
            public static void AddApplicationServices(this IServiceCollection services)
            {
                services.AddAutoMapper(Assembly.GetExecutingAssembly());
                services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
                services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));

            


        }
    }
}
