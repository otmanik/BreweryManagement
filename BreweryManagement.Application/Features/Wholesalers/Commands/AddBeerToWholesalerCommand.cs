using MediatR;
using FluentValidation;
using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Application.Common.Exceptions;
using BreweryManagement.Domain.Entities;

namespace BreweryManagement.Application.Features.Wholesalers.Commands
{
	public class AddBeerToWholesalerCommand : IRequest<int>
	{
		public int WholesalerId { get; set; }
		public int BeerId { get; set; }
		public int Quantity { get; set; }
	}

	public class AddBeerToWholesalerCommandHandler : IRequestHandler<AddBeerToWholesalerCommand, int>
	{
		private readonly IUnitOfWork _unitOfWork;

		public AddBeerToWholesalerCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<int> Handle(AddBeerToWholesalerCommand request, CancellationToken cancellationToken)
		{
			var wholesaler = await _unitOfWork.Wholesalers.GetByIdAsync(request.WholesalerId);
			if (wholesaler == null)
				throw new NotFoundException(nameof(Wholesaler), request.WholesalerId);

			var beer = await _unitOfWork.Beers.GetByIdAsync(request.BeerId);
			if (beer == null)
				throw new NotFoundException(nameof(Beer), request.BeerId);

			var existingStock = wholesaler.Stock.FirstOrDefault(s => s.BeerId == request.BeerId);
			if (existingStock != null)
			{
				existingStock.Quantity += request.Quantity;
			}
			else
			{
				var newStock = new WholesalerStock
				{
					WholesalerId = request.WholesalerId,
					BeerId = request.BeerId,
					Quantity = request.Quantity
				};
				await _unitOfWork.WholesalerStocks.AddAsync(newStock);
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return request.BeerId;
		}
	}

	public class AddBeerToWholesalerCommandValidator : AbstractValidator<AddBeerToWholesalerCommand>
	{
		public AddBeerToWholesalerCommandValidator()
		{
			RuleFor(v => v.WholesalerId)
				.GreaterThan(0).WithMessage("WholesalerId must be greater than 0.");

			RuleFor(v => v.BeerId)
				.GreaterThan(0).WithMessage("BeerId must be greater than 0.");

			RuleFor(v => v.Quantity)
				.GreaterThan(0).WithMessage("Quantity must be greater than 0.");
		}
	}
}