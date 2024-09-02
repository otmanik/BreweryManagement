using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BreweryManagement.Infrastructure.Repositories
{
	public class BeerRepository : Repository<Beer>, IBeerRepository
	{
		public BeerRepository(DbContext context) : base(context)
		{
		}
	}
}