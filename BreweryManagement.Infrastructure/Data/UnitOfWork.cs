using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Infrastructure.Repositories;

namespace BreweryManagement.Infrastructure.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			Breweries = new BreweryRepository(_context);
			Beers = new BeerRepository(_context);
			Wholesalers = new WholesalerRepository(_context);
			WholesalerStocks = new WholesalerStockRepository(_context);
		}

		public IBreweryRepository Breweries { get; private set; }
		public IBeerRepository Beers { get; private set; }
		public IWholesalerRepository Wholesalers { get; private set; }
		public IWholesalerStockRepository WholesalerStocks { get; private set; }

		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await _context.SaveChangesAsync(cancellationToken);
		}
	}
}