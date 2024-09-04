using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Application.Common.Interfaces
{
	public interface IUnitOfWork
	{
		IBreweryRepository Breweries { get; }
		IBeerRepository Beers { get; }
		IWholesalerRepository Wholesalers { get; }
		IWholesalerStockRepository WholesalerStocks { get; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}