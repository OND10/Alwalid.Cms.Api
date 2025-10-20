using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Order.Dtos;
using Alwalid.Cms.Api.Features.Order.Payments;
using Alwalid.Cms.Api.Features.Order.Repositories;
using Alwalid.Cms.Api.Hubs;
using Alwalid.Cms.Api.Middleware;
using Alwalid.Cms.Api.StateMachine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Alwalid.Cms.Api.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static void ConfigureRequestPipeline(this WebApplication app, IWebHostEnvironment env)
		{
			// Configure the HTTP request pipeline.
			//if (env.IsDevelopment())
			//{
			app.UseSwagger();
			app.UseSwaggerUI();
			//}

			// Middleware order is important
			app.UseHttpsRedirection();
			app.UseStatusCodePages();
			app.UseExceptionHandler(); // Must be configured early to catch exceptions
			app.UseMiddleware<RateLimitingMiddleware>();
			app.UseCors("ProductionPolicy");
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
				RequestPath = "/Images"
			});

            app.UseAuthentication();

            app.UseAuthorization();

			// Map endpoints
			app.MapControllers();
            app.MapHub<ChatHub>("/chathub");
            app.MapReverseProxy();

			app.MapApplicationEndpoints();

			// Seed database
			using var scope = app.Services.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			db.Database.EnsureCreated();
			if (!db.Users.Any())
			{
				db.Users.AddRange(
					new User { Email = "user1@absher.com", MarketName = "Absher", ReceiveCategoryNotifications = true },
					new User { Email = "user2@absher.com", MarketName = "Absher", ReceiveCategoryNotifications = true },
					new User { Email = "someone@other.com", MarketName = "OtherMarket", ReceiveCategoryNotifications = true }
				);
				db.SaveChanges();
			}
		}

		public static void MapApplicationEndpoints(this IEndpointRouteBuilder app)
		{
			app.MapPost("/orders", async ([FromBody] CreateOrderDto dto, IOrderRepository repo) =>
			{
				var order = new Order
				{
					CustomerEmail = dto.CustomerEmail,
					Amount = dto.Amount
				};
				await repo.AddAsync(order);
				return Results.Created($"/orders/{order.Id}", new OrderResponse(order));
			});

			app.MapGet("/orders/{id:guid}", async (Guid id, IOrderRepository repo) =>
			{
				var order = await repo.GetAsync(id);
				return order is null ? Results.NotFound() : Results.Ok(new OrderResponse(order));
			});

			app.MapGet("/orders/{id:guid}/permitted", async (Guid id, IOrderRepository repo, IPaymentProbe payments) =>
			{
				var order = await repo.GetAsync(id);
				if (order is null) return Results.NotFound();

				var osm = new OrderStateMachine(order, () => payments.HasPayment(order.Id));
				var permitted = osm.GetPermittedTriggers().Select(p => new { trigger = p.triggers.ToString(), to = p.toStates.ToString() });
				return Results.Ok(permitted);
			});

			app.MapPost("/orders/{id:guid}/pay", (Guid id, IOrderRepository repo, IPaymentProbe payments) => HandleTrigger(id, OrderTrigger.Pay, repo, payments));
			app.MapPost("/orders/{id:guid}/pack", (Guid id, IOrderRepository repo, IPaymentProbe payments) => HandleTrigger(id, OrderTrigger.Pack, repo, payments));
			app.MapPost("/orders/{id:guid}/ship", (Guid id, IOrderRepository repo, IPaymentProbe payments) => HandleTrigger(id, OrderTrigger.Ship, repo, payments));
			app.MapPost("/orders/{id:guid}/deliver", (Guid id, IOrderRepository repo, IPaymentProbe payments) => HandleTrigger(id, OrderTrigger.Deliver, repo, payments));
			app.MapPost("/orders/{id:guid}/cancel", (Guid id, IOrderRepository repo, IPaymentProbe payments) => HandleTrigger(id, OrderTrigger.Cancel, repo, payments));
		}

		private static async Task<IResult> HandleTrigger(Guid id, OrderTrigger trigger, IOrderRepository repo, IPaymentProbe payments)
		{
			var order = await repo.GetAsync(id);
			if (order is null) return Results.NotFound();

			var osm = new OrderStateMachine(order, () => payments.HasPayment(order.Id));

			try
			{
				osm.Fire(trigger);
				await repo.UpdateAsync(order);
				return Results.Ok(new OrderResponse(order));
			}
			catch (InvalidOperationException ex)
			{
				return Results.BadRequest(new { error = ex.Message });
			}
		}
	}
}
