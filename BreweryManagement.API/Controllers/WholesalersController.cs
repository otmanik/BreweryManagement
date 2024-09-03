using Microsoft.AspNetCore.Mvc;
using MediatR;
using BreweryManagement.Application.Features.Wholesalers.Commands;

namespace BreweryManagement.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class WholesalersController : ControllerBase
	{
		private readonly IMediator _mediator;

		public WholesalersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("{wholesalerId}/beers")]
		public async Task<ActionResult<int>> AddBeerToWholesaler(int wholesalerId, AddBeerToWholesalerCommand command)
		{
			if (wholesalerId != command.WholesalerId)
			{
				return BadRequest();
			}

			var result = await _mediator.Send(command);
			return Ok(result);
		}
	}
}