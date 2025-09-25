using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure.Data;

/// <summary>
/// Provides seed data for the database
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    /// <param name="context">The database context</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public static async Task SeedAsync(ChatDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // Check if data already exists
        if (await context.Users.AnyAsync())
        {
            return; // Data already seeded
        }

        // Create seed users
        var users = new[]
        {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@chatapp.com",
                Name = "System Administrator",
                GoogleId = "admin-google-id",
                ProfilePictureUrl = "https://via.placeholder.com/100x100?text=Admin",
                CreatedAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "demo@chatapp.com",
                Name = "Demo User",
                GoogleId = "demo-google-id",
                ProfilePictureUrl = "https://via.placeholder.com/100x100?text=Demo",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                LastActiveAt = DateTime.UtcNow.AddHours(-2)
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        // Create seed messages
        var messages = new[]
        {
            new Message
            {
                Id = Guid.NewGuid(),
                Content = "Welcome to the Chat App! This is a demonstration of real-time messaging with clean architecture.",
                UserId = users[0].Id,
                CreatedAt = DateTime.UtcNow.AddHours(-1)
            },
            new Message
            {
                Id = Guid.NewGuid(),
                Content = "Hello everyone! Excited to be here and test out the messaging features.",
                UserId = users[1].Id,
                CreatedAt = DateTime.UtcNow.AddMinutes(-30)
            },
            new Message
            {
                Id = Guid.NewGuid(),
                Content = "The application supports Google OAuth authentication, real-time messaging via SignalR, and follows clean architecture principles.",
                UserId = users[0].Id,
                CreatedAt = DateTime.UtcNow.AddMinutes(-15)
            },
            new Message
            {
                Id = Guid.NewGuid(),
                Content = "That's awesome! The UI looks clean and the real-time features work great.",
                UserId = users[1].Id,
                CreatedAt = DateTime.UtcNow.AddMinutes(-5)
            }
        };

        await context.Messages.AddRangeAsync(messages);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds the database with initial data if it doesn't exist
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
        
        try
        {
            await SeedAsync(context);
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            throw;
        }
    }
}