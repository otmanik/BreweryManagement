using BreweryManagement.Application.Features.Beers.Commands;
using BreweryManagement.Application.Features.Beers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BreweryManagement.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BeersController : ControllerBase
	{
		private readonly IMediator _mediator;

		public BeersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("brewery/{breweryId}")]
		public async Task<ActionResult<IEnumerable<BeerDto>>> GetBeersByBrewery(int breweryId)
		{
			var query = new GetBeersByBreweryQuery { BreweryId = breweryId };
			var result = await _mediator.Send(query);
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<int>> CreateBeer(CreateBeerCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteBeer(int id)
		{
			var result = await _mediator.Send(new DeleteBeerCommand { Id = id });

			if (result)
			{
				return NoContent();
			}
			else
			{
				return NotFound();
			}
		}
	}
}
