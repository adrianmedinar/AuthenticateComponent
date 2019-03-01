using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Axity.Security.Core.Domain.Entities;
using Axity.Security.Core.Shared;
using Microsoft.AspNetCore.Identity;

namespace Axity.Security.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<RefreshToken>(ConfigureRefreshToken);
        }

        public void ConfigureRefreshToken(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
        }

        public void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(User.RefreshTokens));
            //EF access the RefreshTokens collection property through its backing field
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Ignore(b => b.Email);
            builder.Ignore(b => b.PasswordHash);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuitInfo();
            return await base.SaveChangesAsync();
        }

        private void AddAuitInfo()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).Created = DateTime.UtcNow;
                }
                ((BaseEntity)entry.Entity).Modified = DateTime.UtcNow;
            }
        }
    }
}


