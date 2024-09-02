using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BreweryManagement.Infrastructure.Repositories
{
	public class WholesalerRepository : Repository<Wholesaler>, IWholesalerRepository
	{
		public WholesalerRepository(DbContext context) : base(context)
		{
		}
	}
}