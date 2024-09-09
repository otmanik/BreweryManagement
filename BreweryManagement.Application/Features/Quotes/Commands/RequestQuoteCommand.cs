using MediatR;
using FluentValidation;
using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Application.Common.Exceptions;
using BreweryManagement.Domain.Entities;

namespace BreweryManagement.Application.Features.Quotes.Commands
{
	public class RequestQuoteCommand : IRequest<QuoteResult>
	{
		public int WholesalerId { get; set; }
		public List<OrderItem> OrderItems { get; set; }
	}

	public class OrderItem
	{
		public int BeerId { get; set; }
		public int Quantity { get; set; }
	}

	public class QuoteResult
	{
		public decimal TotalPrice { get; set; }
		public string Summary { get; set; }
	}

	public class RequestQuoteCommandHandler : IRequestHandler<RequestQuoteCommand, QuoteResult>
	{
		private readonly IUnitOfWork _unitOfWork;

		public RequestQuoteCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<QuoteResult> Handle(RequestQuoteCommand request, CancellationToken cancellationToken)
		{
			var wholesaler = await _unitOfWork.Wholesalers.GetByIdAsync(request.WholesalerId);
			if (wholesaler == null)
				throw new NotFoundException(nameof(Wholesaler), request.WholesalerId);

			if (!request.OrderItems.Any())
				throw new ValidationException("The order cannot be empty.");

			if (request.OrderItems.GroupBy(x => x.BeerId).Any(g => g.Count() > 1))
				throw new ValidationException("There can't be any duplicate in the order.");

			decimal totalPrice = 0;
			int totalQuantity = 0;
			var summary = new List<string>();

			foreach (var item in request.OrderItems)
			{
				var stock = await _unitOfWork.WholesalerStocks.FindAsync(
					ws => ws.WholesalerId == request.WholesalerId && ws.BeerId == item.BeerId);
				var wholesalerStock = stock.FirstOrDefault();

				if (wholesalerStock == null)
					throw new ValidationException($"The beer (Id: {item.BeerId}) is not sold by this wholesaler.");

				if (item.Quantity > wholesalerStock.Quantity)
					throw new ValidationException($"The ordered quantity ({item.Quantity}) of beer (Id: {item.BeerId}) is greater than the wholesaler's stock.");

				var beer = await _unitOfWork.Beers.GetByIdAsync(item.BeerId);
				decimal itemPrice = beer.Price * item.Quantity;
				totalPrice += itemPrice;
				totalQuantity += item.Quantity;

				summary.Add($"Beer: {beer.Name}, Quantity: {item.Quantity}, Price: {itemPrice:C}");
			}

			// Apply discounts
			if (totalQuantity > 20)
			{
				totalPrice *= 0.8m;
				summary.Add("20% discount applied for ordering more than 20 drinks.");
			}
			else if (totalQuantity > 10)
			{
				totalPrice *= 0.9m; 
				summary.Add("10% discount applied for ordering more than 10 drinks.");
			}

			summary.Add($"Total Price: {totalPrice:C}");

			return new QuoteResult
			{
				TotalPrice = totalPrice,
				Summary = string.Join("\n", summary)
			};
		}
	}

	public class RequestQuoteCommandValidator : AbstractValidator<RequestQuoteCommand>
	{
		public RequestQuoteCommandValidator()
		{
			RuleFor(x => x.WholesalerId).GreaterThan(0);
			RuleFor(x => x.OrderItems).NotEmpty();
			RuleForEach(x => x.OrderItems).ChildRules(item =>
			{
				item.RuleFor(x => x.BeerId).GreaterThan(0);
				item.RuleFor(x => x.Quantity).GreaterThan(0);
			});
		}
	}
}