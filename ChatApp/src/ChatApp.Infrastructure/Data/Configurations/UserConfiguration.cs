using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for the User entity
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the User entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.GoogleId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.ProfilePictureUrl)
            .HasMaxLength(500);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.LastActiveAt)
            .IsRequired();

        builder.Property(u => u.DeletedAt);

        // Indexes
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        builder.HasIndex(u => u.GoogleId)
            .IsUnique()
            .HasDatabaseName("IX_Users_GoogleId");

        builder.HasIndex(u => u.CreatedAt)
            .HasDatabaseName("IX_Users_CreatedAt");

        builder.HasIndex(u => u.LastActiveAt)
            .HasDatabaseName("IX_Users_LastActiveAt");

        // Relationships
        builder.HasMany(u => u.Messages)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}