using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using ChatApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure;

/// <summary>
/// Extension methods for configuring infrastructure services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds infrastructure services to the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Entity Framework
        services.AddDbContext<ChatDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString, b => 
            {
                b.MigrationsAssembly(typeof(ChatDbContext).Assembly.FullName);
                b.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            });

            // Enable sensitive data logging in development
            if (configuration.GetValue<bool>("DetailedErrors"))
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    /// <summary>
    /// Ensures the database is created and migrated
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public static async Task EnsureDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();

        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
            throw;
        }
    }
}