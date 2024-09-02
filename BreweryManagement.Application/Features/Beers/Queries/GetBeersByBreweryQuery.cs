using MediatR;
using AutoMapper;
using BreweryManagement.Application.Common.Interfaces;

namespace BreweryManagement.Application.Features.Beers.Queries
{
	public class GetBeersByBreweryQuery : IRequest<List<BeerDto>>
	{
		public int BreweryId { get; set; }
	}

	public class GetBeersByBreweryQueryHandler : IRequestHandler<GetBeersByBreweryQuery, List<BeerDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetBeersByBreweryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<BeerDto>> Handle(GetBeersByBreweryQuery request, CancellationToken cancellationToken)
		{
			var beers = await _unitOfWork.Breweries.GetBeersByBreweryAsync(request.BreweryId);
			return _mapper.Map<List<BeerDto>>(beers);
		}
	}

	public class BeerDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal AlcoholContent { get; set; }
		public decimal Price { get; set; }
	}
}