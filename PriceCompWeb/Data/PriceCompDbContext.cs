using Microsoft.EntityFrameworkCore;
using PriceCompWeb.Models;

namespace PriceCompWeb.Data;

public class PriceCompDbContext : DbContext
{
    public PriceCompDbContext(DbContextOptions<PriceCompDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<PageContent> PageContents => Set<PageContent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasMany(product => product.Offers)
            .WithOne(offer => offer.Product)
            .HasForeignKey(offer => offer.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Store>()
            .HasMany(store => store.Offers)
            .WithOne(offer => offer.Store)
            .HasForeignKey(offer => offer.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PageContent>()
            .HasIndex(content => content.Key)
            .IsUnique();
    }
}
