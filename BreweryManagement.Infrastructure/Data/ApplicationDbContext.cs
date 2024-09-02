using Microsoft.EntityFrameworkCore;
using BreweryManagement.Domain.Entities;

namespace BreweryManagement.Infrastructure.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Beer> Beers { get; set; }
		public DbSet<Brewery> Breweries { get; set; }
		public DbSet<Wholesaler> Wholesalers { get; set; }
		public DbSet<WholesalerStock> WholesalerStocks { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

			// Define relationships
			modelBuilder.Entity<Beer>()
				.HasOne(b => b.Brewery)
				.WithMany(br => br.Beers)
				.HasForeignKey(b => b.BreweryId);

			modelBuilder.Entity<WholesalerStock>()
				.HasOne(ws => ws.Wholesaler)
				.WithMany(w => w.Stock)
				.HasForeignKey(ws => ws.WholesalerId);

			modelBuilder.Entity<WholesalerStock>()
				.HasOne(ws => ws.Beer)
				.WithMany()
				.HasForeignKey(ws => ws.BeerId);
		}
	}
}