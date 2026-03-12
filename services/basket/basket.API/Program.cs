
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using basket.Application.Commands;
using basket.Application.GrpcServices;
using basket.Application.Mappers;
using basket.Core.Repositories;
using basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog(Logging.ConfigureLogging);

            builder.Services.AddControllers();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.Authority = "https://host.docker.internal:9009";
                   options.RequireHttpsMetadata = true;
                   options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = "https://localhost:9009",
                       ValidateAudience = true,
                       ValidAudience = "Basket",
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ClockSkew = TimeSpan.Zero
                   };
                   // add this to bypass SSL certificate validation for development purposes only, not recommended for production environments
                   options.BackchannelHttpHandler = new HttpClientHandler
                   {
                       ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                   };

                   options.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                           Log.Error("Authentication failed: {ErrorMessage}", context.Exception.Message);
                           return Task.CompletedTask;
                       },
                       OnTokenValidated = context =>
                       {
                           Log.Information("Token validated successfully for user: {UserName}", context.Principal.Identity.Name);
                           return Task.CompletedTask;
                       },
                       OnChallenge = context =>
                       {
                           Log.Warning("Authentication challenge: {ErrorDescription}", context.ErrorDescription);
                           return Task.CompletedTask;
                       }
                   };
               }
               );

            builder.Services.AddAutoMapper(typeof(BasketMappingProfile).Assembly);
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(CreateShoppingCartCommand))));

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<DiscountGrpcService>();
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
                cfg=>cfg.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));

            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                });
            });
            builder.Services.AddMassTransitHostedService();

            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Basket.API",
                    Version = "v1",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "hima Elarby",
                        Email = "Ibrahim.elarby.scurt@gmail.com",
                    }
                });
             options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
             {
                 Title = "Basket.API",
                 Version = "v2",
                 Contact = new Microsoft.OpenApi.Models.OpenApiContact
                 {
                     Name = "hima Elarby",
                     Email = "Ibrahim.elarby.scurt@gmail.com",
                 }
             });
              
            });

            var userPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
              .RequireAuthenticatedUser()
              .Build();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(userPolicy));
            });
            //redis 
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Basket.API v2");
                });
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
