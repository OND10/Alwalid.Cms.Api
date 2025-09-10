using Alwalid.Cms.Api.Common.Exception;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Features.Category.Dtos;
using Alwalid.Cms.Api.Features.Order.Payments;
using Alwalid.Cms.Api.Features.Order.Repositories;
using Alwalid.Cms.Api.Settings;
using FluentValidation;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

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
			services.AddCors(options =>
			{
				options.AddPolicy("ProductionPolicy", policy =>
				{
					policy.WithOrigins("http://localhost:5173")
						  .WithMethods("GET", "POST", "PUT", "DELETE")
						  .WithHeaders("Content-Type")
						  .AllowCredentials()
						  .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
				});
			});

			// Register the global exception handler into app container
			services.AddExceptionHandler<GlobalExceptionHandler>();

			// Add Memory Cache
			services.AddMemoryCache();

			// Add HttpContextAccessor
			services.AddHttpContextAccessor();

			// Add DbContext
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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
