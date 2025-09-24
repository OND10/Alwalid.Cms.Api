namespace ChatApp.Domain.Common;

/// <summary>
/// Base entity class that provides common properties for all domain entities
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the timestamp when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp when the entity was soft deleted (nullable)
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Gets a value indicating whether the entity is deleted
    /// </summary>
    public bool IsDeleted => DeletedAt.HasValue;

    /// <summary>
    /// Marks the entity as deleted by setting the DeletedAt timestamp
    /// </summary>
    public void MarkAsDeleted()
    {
        DeletedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Restores a soft-deleted entity by clearing the DeletedAt timestamp
    /// </summary>
    public void Restore()
    {
        DeletedAt = null;
    }
}