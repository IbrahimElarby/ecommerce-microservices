
using catalog.Application.Mappers;
using catalog.Application.Queries;
using catalog.Core.Repositories;
using catalog.Infrastructure.Data.Contexts;
using catalog.Infrastructure.Repositories;
using Common.Logging;
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

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(),Assembly.GetAssembly(typeof(GetAllProductsByIdQuery))));
            builder.Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
            builder.Services.AddScoped<ICatalogContext, CatalogContext>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductBrandRepository, ProductRepository>();
            builder.Services.AddScoped<IProductTypeRepository, ProductRepository>();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
