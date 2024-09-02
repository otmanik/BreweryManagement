using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Domain.Entities;
using BreweryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Infrastructure.Repositories
{
	public class BreweryRepository : Repository<Brewery>, IBreweryRepository
	{
		public BreweryRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Beer>> GetBeersByBreweryAsync(int breweryId)
		{
			return await _context.Beers
				.Where(b => b.BreweryId == breweryId)
				.ToListAsync();
		}
	}
}