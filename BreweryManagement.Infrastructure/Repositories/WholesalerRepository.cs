using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Domain.Entities;
using BreweryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BreweryManagement.Infrastructure.Repositories
{
	public class WholesalerRepository : Repository<Wholesaler>, IWholesalerRepository
	{
		public WholesalerRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}