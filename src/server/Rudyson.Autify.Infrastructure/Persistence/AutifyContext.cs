using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rudyson.Autify.Domain.Entities;
using Rudyson.Autify.Domain.Repositories;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Infrastructure.Persistence;

public class AutifyContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Session> Sessions => Set<Session>();

    public AutifyContext(DbContextOptions<AutifyContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutifyContext).Assembly);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasConversion(id => id.Value, v => new UserId(v));
        builder.Property(u => u.Email).HasConversion(e => e.Value, v => new Email(v));
        builder.Property(u => u.PasswordHash).HasConversion(p => p.Value, v => new PasswordHash(v));
        builder.HasIndex(u => u.Email).IsUnique();
    }
}

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasConversion(id => id.Value, v => new SessionId(v));
        builder.Property(s => s.UserId).HasConversion(id => id.Value, v => new UserId(v));
        builder.Property(s => s.RefreshTokenHash).HasConversion(t => t.Value, v => new RefreshTokenHash(v));
        builder.HasOne<User>().WithMany().HasForeignKey(s => s.UserId);
    }
}

