using MediatR;
using FluentValidation;
using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Application.Common.Exceptions;
using BreweryManagement.Domain.Entities;

namespace BreweryManagement.Application.Features.Wholesalers.Commands
{
	public class UpdateBeerStockCommand : IRequest<Unit>
	{
		public int WholesalerId { get; set; }
		public int BeerId { get; set; }
		public int NewQuantity { get; set; }
	}

	public class UpdateBeerStockCommandHandler : IRequestHandler<UpdateBeerStockCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateBeerStockCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(UpdateBeerStockCommand request, CancellationToken cancellationToken)
		{
			var wholesaler = await _unitOfWork.Wholesalers.GetByIdAsync(request.WholesalerId);
			if (wholesaler == null)
				throw new NotFoundException(nameof(Wholesaler), request.WholesalerId);

			var beer = await _unitOfWork.Beers.GetByIdAsync(request.BeerId);
			if (beer == null)
				throw new NotFoundException(nameof(Beer), request.BeerId);

			var stock = await _unitOfWork.WholesalerStocks.FindAsync(s => s.WholesalerId == request.WholesalerId && s.BeerId == request.BeerId);
			var existingStock = stock.FirstOrDefault();

			if (existingStock == null)
				throw new NotFoundException($"Stock for Beer {request.BeerId} not found for Wholesaler {request.WholesalerId}");

			existingStock.Quantity = request.NewQuantity;
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}

	public class UpdateBeerStockCommandValidator : AbstractValidator<UpdateBeerStockCommand>
	{
		public UpdateBeerStockCommandValidator()
		{
			RuleFor(v => v.WholesalerId)
				.GreaterThan(0).WithMessage("WholesalerId must be greater than 0.");

			RuleFor(v => v.BeerId)
				.GreaterThan(0).WithMessage("BeerId must be greater than 0.");

			RuleFor(v => v.NewQuantity)
				.GreaterThanOrEqualTo(0).WithMessage("NewQuantity must be non-negative.");
		}
	}
}