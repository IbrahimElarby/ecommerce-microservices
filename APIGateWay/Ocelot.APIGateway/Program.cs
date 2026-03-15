using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var authschema = "EshoppingGatewayAuthSchema";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(authschema, options =>
              {
                  options.Authority = "https://host.docker.internal:9009";
                  options.RequireHttpsMetadata = true;
                  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidIssuer = "https://localhost:9009",
                      ValidateAudience = true,
                      ValidAudience = "EShoppingGateway",
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


builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello ocelot");
    });
});

await app.UseOcelot();
await app.RunAsync();
