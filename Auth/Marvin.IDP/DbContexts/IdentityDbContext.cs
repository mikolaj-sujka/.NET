using Marvin.IDP.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marvin.IDP.DbContexts
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Subject)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                    Password = "password",
                    Subject = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    UserName = "David",
                    Email = "david@example.com",
                    Active = true,
                    ConcurrencyStamp = "11111111-1111-1111-1111-111111111111"
                },
                new User()
                {
                    Id = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                    Password = "password",
                    Subject = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    UserName = "Emma",
                    Email = "emma@example.com",
                    Active = true,
                    ConcurrencyStamp = "22222222-2222-2222-2222-222222222222"
                });

            modelBuilder.Entity<UserClaim>().HasData(
                new UserClaim()
                {
                    Id = new Guid("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"),
                    UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                    Type = "given_name",
                    Value = "David",
                    ConcurrencyStamp = "33333333-3333-3333-3333-333333333333"
                },
                new UserClaim()
                {
                    Id = new Guid("b2c3d4e5-f6a7-4b5c-9d0e-1f2a3b4c5d6e"),
                    UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                    Type = "family_name",
                    Value = "Flagg",
                    ConcurrencyStamp = "44444444-4444-4444-4444-444444444444"
                },
                new UserClaim()
                {
                    Id = new Guid("c3d4e5f6-a7b8-4c5d-0e1f-2a3b4c5d6e7f"),
                    UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                    Type = "country",
                    Value = "nl",
                    ConcurrencyStamp = "55555555-5555-5555-5555-555555555555"
                },
                new UserClaim()
                {
                    Id = new Guid("d4e5f6a7-b8c9-4d5e-1f2a-3b4c5d6e7f8a"),
                    UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                    Type = "role",
                    Value = "FreeUser",
                    ConcurrencyStamp = "66666666-6666-6666-6666-666666666666"
                },
                new UserClaim()
                {
                    Id = new Guid("e5f6a7b8-c9d0-4e5f-2a3b-4c5d6e7f8a9b"),
                    UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                    Type = "given_name",
                    Value = "Emma",
                    ConcurrencyStamp = "77777777-7777-7777-7777-777777777777"
                },
                new UserClaim()
                {
                    Id = new Guid("f6a7b8c9-d0e1-4f5a-3b4c-5d6e7f8a9b0c"),
                    UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                    Type = "family_name",
                    Value = "Flagg",
                    ConcurrencyStamp = "88888888-8888-8888-8888-888888888888"
                },
                new UserClaim()
                {
                    Id = new Guid("a7b8c9d0-e1f2-4a5b-4c5d-6e7f8a9b0c1d"),
                    UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                    Type = "country",
                    Value = "be",
                    ConcurrencyStamp = "99999999-9999-9999-9999-999999999999"
                },
                new UserClaim()
                {
                    Id = new Guid("b8c9d0e1-f2a3-4b5c-5d6e-7f8a9b0c1d2e"),
                    UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                    Type = "role",
                    Value = "PayingUser",
                    ConcurrencyStamp = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"
                });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var updatedConcurrencyAwareEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .OfType<IConcurrencyAware>();

            foreach (var entry in updatedConcurrencyAwareEntries)
            {
                entry.ConcurrencyStamp = Guid.NewGuid().ToString();
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
