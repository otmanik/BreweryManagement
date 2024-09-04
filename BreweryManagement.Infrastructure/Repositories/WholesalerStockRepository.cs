using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Domain.Entities;
using BreweryManagement.Infrastructure.Data;

namespace BreweryManagement.Infrastructure.Repositories
{
	public class WholesalerStockRepository : Repository<WholesalerStock>, IWholesalerStockRepository
	{
		public WholesalerStockRepository(ApplicationDbContext context) : base(context)
		{
		}

	}
}