using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;


namespace BreweryManagement.Application
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddMediatR(Assembly.GetExecutingAssembly());
			return services;
		}
	}
}
