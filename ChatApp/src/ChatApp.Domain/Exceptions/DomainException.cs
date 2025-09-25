namespace ChatApp.Domain.Exceptions;

/// <summary>
/// Base exception class for domain-specific exceptions
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// Initializes a new instance of the DomainException class
    /// </summary>
    /// <param name="message">The exception message</param>
    protected DomainException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the DomainException class
    /// </summary>
    /// <param name="message">The exception message</param>
    /// <param name="innerException">The inner exception</param>
    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when an entity is not found
/// </summary>
public class EntityNotFoundException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the EntityNotFoundException class
    /// </summary>
    /// <param name="entityName">The name of the entity</param>
    /// <param name="entityId">The identifier of the entity</param>
    public EntityNotFoundException(string entityName, object entityId)
        : base($"{entityName} with ID '{entityId}' was not found.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }

    /// <summary>
    /// Gets the name of the entity that was not found
    /// </summary>
    public string EntityName { get; }

    /// <summary>
    /// Gets the identifier of the entity that was not found
    /// </summary>
    public object EntityId { get; }
}

/// <summary>
/// Exception thrown when unauthorized access is attempted
/// </summary>
public class UnauthorizedAccessException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the UnauthorizedAccessException class
    /// </summary>
    /// <param name="message">The exception message</param>
    public UnauthorizedAccessException(string message) : base(message)
    {
    }
}

/// <summary>
/// Exception thrown when a business rule validation fails
/// </summary>
public class BusinessRuleValidationException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the BusinessRuleValidationException class
    /// </summary>
    /// <param name="message">The exception message</param>
    public BusinessRuleValidationException(string message) : base(message)
    {
    }
}