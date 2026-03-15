
using catalog.Application.Mappers;
using catalog.Application.Queries;
using catalog.Core.Repositories;
using catalog.Infrastructure.Data.Contexts;
using catalog.Infrastructure.Repositories;
using Common.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using System.Reflection;

namespace catalog.API
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
                .AddJwtBearer(options=>
                {
                    options.Authority = "https://host.docker.internal:9009";
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://localhost:9009",
                        ValidateAudience = true,
                        ValidAudience = "Catalog",
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

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ReadAccess", policy => policy.RequireClaim("scope", "catalogapi.read"));
                options.AddPolicy("WriteAccess", policy => policy.RequireClaim("scope", "catalogapi.write"));
            });
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(),Assembly.GetAssembly(typeof(GetAllProductsByIdQuery))));
            builder.Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
            builder.Services.AddScoped<ICatalogContext, CatalogContext>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductBrandRepository, ProductRepository>();
            builder.Services.AddScoped<IProductTypeRepository, ProductRepository>();

            var userPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(userPolicy));
            });

            builder.Services.AddApiVersioning(options=>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1,0);
                options.ReportApiVersions = true;
            });

            builder.Services.AddSwaggerGen(options =>
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Catalog.API",
                Version = "v1",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "hima Elarby",
                    Email = "Ibrahim.elarby.scurt@gmail.com",
                }
            }));

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
