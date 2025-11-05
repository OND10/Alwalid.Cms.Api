using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for the Message entity
/// </summary>
public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    /// <summary>
    /// Configures the Message entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(m => m.UserId)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.EditedAt);

        builder.Property(m => m.DeletedAt);

        // Indexes
        builder.HasIndex(m => m.UserId)
            .HasDatabaseName("IX_Messages_UserId");

        builder.HasIndex(m => m.CreatedAt)
            .HasDatabaseName("IX_Messages_CreatedAt");

        builder.HasIndex(m => new { m.CreatedAt, m.DeletedAt })
            .HasDatabaseName("IX_Messages_CreatedAt_DeletedAt");

        // Relationships
        builder.HasOne(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}