using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BreweryManagement.Infrastructure
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}
}