using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Shared;
using Alwalid.Cms.Api.Events;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Alwalid.Cms.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IEventPublisher _publisher;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IEventPublisher publisher) : base(options)
        {
            _publisher = publisher;

        }

        // DbSet properties
        public DbSet<Country> Countries { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductStatistic> ProductStatistics { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Entities.Settings> Settings { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Partners> Partners { get; set; }
        public DbSet<Entities.Services> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
           .HasMany(u => u.Conversations)
           .WithOne(c => c.ApplicationUser)
           .HasForeignKey(c => c.ApplicationUserId);

            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Saves changes then publishes all domain events collected on tracked entities.
        /// </summary>
        public async Task<int> SaveChangesWithEventsAsync(CancellationToken ct = default)
        {
            var domainEvents = ChangeTracker
                .Entries<BaseEntity>()
                .Select(e => e.Entity)
                .SelectMany(e => e.DomainEvents)
                .ToList();

            var result = await base.SaveChangesAsync(ct);

            if (domainEvents.Count > 0)
            {
                await _publisher.PublishAsync(domainEvents, ct);

                foreach (var entityEntry in ChangeTracker.Entries<BaseEntity>())
                {
                    entityEntry.Entity.ClearDomainEvents();
                }
            }

            return result;
        }
    }
}
