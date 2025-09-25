using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ChatApp.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for coordinating repository operations
/// </summary>
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ChatDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed = false;

    private IUserRepository? _users;
    private IMessageRepository? _messages;
    private IRefreshTokenRepository? _refreshTokens;

    /// <summary>
    /// Initializes a new instance of the UnitOfWork class
    /// </summary>
    /// <param name="context">The database context</param>
    public UnitOfWork(ChatDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public IUserRepository Users => _users ??= new UserRepository(_context);

    /// <inheritdoc />
    public IMessageRepository Messages => _messages ??= new MessageRepository(_context);

    /// <inheritdoc />
    public IRefreshTokenRepository RefreshTokens => _refreshTokens ??= new RefreshTokenRepository(_context);

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            // Log the exception if needed
            throw;
        }
    }

    /// <inheritdoc />
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction in progress.");
        }

        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <inheritdoc />
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction in progress.");
        }

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Disposes the Unit of Work and its resources
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the Unit of Work and its resources
    /// </summary>
    /// <param name="disposing">True if disposing managed resources</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}