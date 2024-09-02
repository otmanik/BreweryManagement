using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Domain.Entities;
using BreweryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BreweryManagement.Infrastructure.Repositories
{
	public class BeerRepository : Repository<Beer>, IBeerRepository
	{
		public BeerRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}