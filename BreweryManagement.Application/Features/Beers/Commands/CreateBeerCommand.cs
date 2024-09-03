using MediatR;
using FluentValidation;
using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Domain.Entities;

namespace BreweryManagement.Application.Features.Beers.Commands
{
	public class CreateBeerCommand : IRequest<int>
	{
		public string Name { get; set; }
		public decimal AlcoholContent { get; set; }
		public decimal Price { get; set; }
		public int BreweryId { get; set; }
	}

	public class CreateBeerCommandHandler : IRequestHandler<CreateBeerCommand, int>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateBeerCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<int> Handle(CreateBeerCommand request, CancellationToken cancellationToken)
		{
			var beer = new Beer
			{
				Name = request.Name,
				AlcoholContent = request.AlcoholContent,
				Price = request.Price,
				BreweryId = request.BreweryId
			};

			await _unitOfWork.Beers.AddAsync(beer);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return beer.Id;
		}
	}

	public class CreateBeerCommandValidator : AbstractValidator<CreateBeerCommand>
	{
		public CreateBeerCommandValidator()
		{
			RuleFor(v => v.Name)
				.NotEmpty().WithMessage("Name is required.")
				.MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

			RuleFor(v => v.AlcoholContent)
				.GreaterThanOrEqualTo(0).WithMessage("Alcohol content must be non-negative.")
				.LessThanOrEqualTo(100).WithMessage("Alcohol content must not exceed 100%.");

			RuleFor(v => v.Price)
				.GreaterThan(0).WithMessage("Price must be greater than zero.");

			RuleFor(v => v.BreweryId)
				.GreaterThan(0).WithMessage("Valid BreweryId is required.");
		}
	}
}