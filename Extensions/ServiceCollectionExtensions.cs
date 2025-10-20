using System.Text;
using Alwalid.Cms.Api.Common.Exception;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Category.Dtos;
using Alwalid.Cms.Api.Features.Conversation.Services;
using Alwalid.Cms.Api.Features.Order.Payments;
using Alwalid.Cms.Api.Features.Order.Repositories;
using Alwalid.Cms.Api.Services;
using Alwalid.Cms.Api.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Alwalid.Cms.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add controllers and OData support
            services.AddControllers().AddOData(opt =>
            {
                opt.Select().Filter().OrderBy();
            }).AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = ctx =>
                {
                    ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
                    ctx.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
                    ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
                };
            });

            // Register settings
            services.Configure<GeminiSettings>(configuration.GetSection(nameof(GeminiSettings)));

            // Add CORS
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("ProductionPolicy", policy =>
            //    {
            //        policy.WithOrigins("http://127.0.0.1:5500")
            //              .WithMethods("GET", "POST", "PUT", "DELETE")
            //              .WithHeaders("Content-Type")
            //              .AllowCredentials()
            //              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
            //    });
            //});

            services.AddCors(options =>
            {
                options.AddPolicy("ProductionPolicy", policy =>
                {
                    policy.WithOrigins("http://127.0.0.1:5500", "null") // "null" is for local file:// origins
                          .AllowAnyMethod()    // More flexible for SignalR and Fetch
                          .AllowAnyHeader()    // Allows Authorization header
                          .AllowCredentials(); // Required for SignalR with auth
                });
            });




            // Register the global exception handler into app container
            services.AddExceptionHandler<GlobalExceptionHandler>();

            // Add Memory Cache
            services.AddMemoryCache();

            // Add HttpContextAccessor
            services.AddHttpContextAccessor();

            // Add DbContext
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              // --- THIS IS THE NEW CODE BLOCK TO ADD ---
              // We need to check for the token in the query string for SignalR
              options.Events = new JwtBearerEvents
              {
                  OnMessageReceived = context =>
                  {
                      // The token is passed as a query string parameter named "access_token"
                      var accessToken = context.Request.Query["access_token"];

                      // If the request is for our hub...
                      var path = context.HttpContext.Request.Path;
                      if (!string.IsNullOrEmpty(accessToken) &&
                          (path.StartsWithSegments("/chathub")))
                      {
                          // Read the token from the query string
                          context.Token = accessToken;
                      }
                      return Task.CompletedTask;
                  }
              };
              // --- END OF NEW CODE BLOCK ---

              // Your existing TokenValidationParameters remain the same
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                  ValidIssuer = configuration["Jwt:Issuer"],
                  ValidAudience = configuration["Jwt:Audience"],
              };
          });

            services.AddAuthorization();

            services.AddSignalR();


            // Register validators from the assembly
            services.AddValidatorsFromAssemblyContaining<CategoryRequestDtoValidator>();

            // Add Application Services (original AddApplication() is kept if it contains other services)
            services.AddApplication();

            // Add Repositories and other services
            services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
            services.AddSingleton<IPaymentProbe, FakePaymentProbe>();

            // Learn more about configuring Swagger/OpenAPI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"));



            return services;
        }
    }
}
