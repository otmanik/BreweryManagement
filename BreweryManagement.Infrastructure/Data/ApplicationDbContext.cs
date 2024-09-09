using Microsoft.EntityFrameworkCore;
using BreweryManagement.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BreweryManagement.Infrastructure.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Beer> Beers { get; set; }
		public DbSet<Brewery> Breweries { get; set; }
		public DbSet<Wholesaler> Wholesalers { get; set; }
		public DbSet<WholesalerStock> WholesalerStocks { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

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

		public static void SeedData(IServiceProvider serviceProvider)
		{
			using (var context = new ApplicationDbContext(
				serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
			{
				if (context.Beers.Any())
				{
					return;
				}

				var abbeyeDeLeffe = new Brewery { Name = "Abbaye de Leffe" };
				var brewDog = new Brewery { Name = "BrewDog" };
				context.Breweries.AddRange(abbeyeDeLeffe, brewDog);

				context.Beers.AddRange(
					new Beer { Name = "Leffe Blonde", AlcoholContent = 6.6m, Price = 2.20m, Brewery = abbeyeDeLeffe },
					new Beer { Name = "Leffe Brune", AlcoholContent = 6.5m, Price = 2.30m, Brewery = abbeyeDeLeffe },
					new Beer { Name = "Punk IPA", AlcoholContent = 5.6m, Price = 3.00m, Brewery = brewDog },
					new Beer { Name = "Dead Pony Club", AlcoholContent = 3.8m, Price = 2.80m, Brewery = brewDog }
				);

				var geneDrinks = new Wholesaler { Name = "GeneDrinks" };
				var beerLovers = new Wholesaler { Name = "Beer Lovers" };
				context.Wholesalers.AddRange(geneDrinks, beerLovers);

				context.SaveChanges();

				context.WholesalerStocks.AddRange(
					new WholesalerStock { Wholesaler = geneDrinks, Beer = context.Beers.First(b => b.Name == "Leffe Blonde"), Quantity = 100 },
					new WholesalerStock { Wholesaler = geneDrinks, Beer = context.Beers.First(b => b.Name == "Punk IPA"), Quantity = 200 },
					new WholesalerStock { Wholesaler = beerLovers, Beer = context.Beers.First(b => b.Name == "Leffe Brune"), Quantity = 150 },
					new WholesalerStock { Wholesaler = beerLovers, Beer = context.Beers.First(b => b.Name == "Dead Pony Club"), Quantity = 250 }
				);

				context.SaveChanges();
			}
		}
	}
}