using MediatR;
using FluentValidation;
using BreweryManagement.Application.Common.Interfaces;

namespace BreweryManagement.Application.Features.Beers.Commands
{
	public class DeleteBeerCommand : IRequest<bool>
	{
		public int Id { get; set; }
	}

	public class DeleteBeerCommandHandler : IRequestHandler<DeleteBeerCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteBeerCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteBeerCommand request, CancellationToken cancellationToken)
		{
			var beer = await _unitOfWork.Beers.GetByIdAsync(request.Id);

			if (beer == null)
			{
				return false;
			}

			await _unitOfWork.Beers.DeleteAsync(beer);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return true;
		}
	}

	public class DeleteBeerCommandValidator : AbstractValidator<DeleteBeerCommand>
	{
		public DeleteBeerCommandValidator()
		{
			RuleFor(v => v.Id)
				.GreaterThan(0).WithMessage("BeerId must be greater than 0.");
		}
	}
}