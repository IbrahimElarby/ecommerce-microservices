using Common.Logging;
using discount.API.Services;
using discount.Application.Commands;
using discount.Application.Mapper;
using discount.Core.Repository;
using discount.Infrastructure.Extensions;
using discount.Infrastructure.Repository;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog(Logging.ConfigureLogging);

builder.Services.AddAutoMapper(typeof(DiscountProfile).Assembly);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(CreateDiscountCommand))));

builder.Services.AddScoped<ICouponRepository,DiscountRepository>();
builder.Services.AddGrpc();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
}

app.MigrateDataBase<Program>();
app.UseRouting();

app.UseEndpoints(endpoints =>
{     
    endpoints.MapGrpcService<DiscountService>();
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    });
});



app.Run();
