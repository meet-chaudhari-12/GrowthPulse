using GrowthPulse.Models;
using Microsoft.EntityFrameworkCore;

namespace GrowthPulse.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- User Configuration (No changes needed here) ---
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasMany(u => u.Plants)
                      .WithOne(p => p.User)
                      .HasForeignKey(p => p.UserId);

                entity.HasMany(u => u.Orders)
                      .WithOne(o => o.Buyer)
                      .HasForeignKey(o => o.BuyerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // --- Plant Configuration ---
            modelBuilder.Entity<Plant>(entity =>
            {
                entity.HasKey(p => p.Id);

                // --- ADD THIS RELATIONSHIP (The optional one you implemented) ---
                // A Plant can optionally originate from one Listing
                entity.HasOne(p => p.Listing)
                      .WithMany() // A Listing can be the source of many Plants
                      .HasForeignKey(p => p.ListingId)
                      .OnDelete(DeleteBehavior.SetNull); // If store listing is deleted, don't delete user's plant
            });

            // --- Listing Configuration ---
            modelBuilder.Entity<Listing>(entity =>
            {
                entity.Property(l => l.Price).HasColumnType("decimal(18,2)");

                // --- ADD THIS RELATIONSHIP ---
                // A Listing can be associated with many OrderItems
                entity.HasMany<OrderItem>()
                      .WithOne(oi => oi.Listing)
                      .HasForeignKey(oi => oi.ListingId);
            });

            // --- Order Configuration ---
            modelBuilder.Entity<Order>(entity =>
            {
                // --- CHANGE THIS ---
                // Configure decimal precision for the NEW TotalPrice property
                entity.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");

                // --- REMOVE THIS (or it will cause an error) ---
                // entity.Property(o => o.FinalPrice).HasColumnType("decimal(18,2)");

                // --- ADD THIS RELATIONSHIP ---
                // An Order has many OrderItems
                entity.HasMany(o => o.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId);
            });

            // --- ADD THIS NEW CONFIGURATION FOR ORDERITEM ---
            modelBuilder.Entity<OrderItem>(entity =>
            {
                // Configure decimal precision for UnitPrice
                entity.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
            });
        }
    }
}