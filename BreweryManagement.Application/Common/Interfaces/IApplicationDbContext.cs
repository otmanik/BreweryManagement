using BreweryManagement.Domain.Entities;
using System.Collections.Generic;

namespace BreweryManagement.Application.Common.Interfaces
{
	public interface IApplicationDbContext
	{
		DbSet<Brewery> Breweries { get; set; }
		DbSet<Beer> Beers { get; set; }
		DbSet<Wholesaler> Wholesalers { get; set; }
		DbSet<WholesalerStock> WholesalerStocks { get; set; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}