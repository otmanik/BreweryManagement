using System.Linq.Expressions;
using BreweryManagement.Application.Common.Interfaces;
using BreweryManagement.Application.Features.Quotes.Commands;
using BreweryManagement.Domain.Entities;
using Moq;
using FluentValidation;

namespace BreweryManagement.UnitTests.Features.Quotes
{
	public class RequestQuoteCommandHandlerTests
	{
		private readonly Mock<IUnitOfWork> _mockUnitOfWork;
		private readonly RequestQuoteCommandHandler _handler;

		public RequestQuoteCommandHandlerTests()
		{
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_handler = new RequestQuoteCommandHandler(_mockUnitOfWork.Object);
		}

		[Fact]
		public async Task Handle_WithLargeOrder_AppliesCorrectDiscount()
		{
			var wholesalerId = 1;
			var beerId = 1;
			var command = new RequestQuoteCommand
			{
				WholesalerId = wholesalerId,
				OrderItems = new List<OrderItem> { new OrderItem { BeerId = beerId, Quantity = 25 } }
			};

			SetupMockData(wholesalerId, beerId);

			var result = await _handler.Handle(command, CancellationToken.None);

			Assert.NotNull(result);
			Assert.Equal(100, result.TotalPrice);
			Assert.Contains("20% discount applied", result.Summary.ToLower());
		}

		private void SetupMockData(int wholesalerId, int beerId)
		{
			_mockUnitOfWork.Setup(uow => uow.Wholesalers.GetByIdAsync(wholesalerId))
				.ReturnsAsync(new Wholesaler { Id = wholesalerId, Name = "Test Wholesaler" });

			_mockUnitOfWork.Setup(uow => uow.Beers.GetByIdAsync(beerId))
				.ReturnsAsync(new Beer { Id = beerId, Name = "Test Beer", Price = 5 });

			_mockUnitOfWork.Setup(uow => uow.WholesalerStocks.FindAsync(It.IsAny<Expression<Func<WholesalerStock, bool>>>()))
				.ReturnsAsync(new List<WholesalerStock>
				{
					new WholesalerStock { WholesalerId = wholesalerId, BeerId = beerId, Quantity = 100 }
				});
		}

		[Fact]
		public async Task Handle_OrderExceedsStock_ThrowsException()
		{
			var wholesalerId = 1;
			var beerId = 1;
			var stockQuantity = 10;
			var orderQuantity = 15; 

			var command = new RequestQuoteCommand
			{
				WholesalerId = wholesalerId,
				OrderItems = new List<OrderItem>
		{
			new OrderItem { BeerId = beerId, Quantity = orderQuantity }
		}
			};

			SetupMockData(wholesalerId, beerId, stockQuantity);

			var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

			Assert.Contains($"ordered quantity ({orderQuantity}) of beer (Id: {beerId}) is greater than the wholesaler's stock.",
				exception.Message);
		}

		private void SetupMockData(int wholesalerId, int beerId, int stockQuantity)
		{
			_mockUnitOfWork.Setup(uow => uow.Wholesalers.GetByIdAsync(wholesalerId))
				.ReturnsAsync(new Wholesaler { Id = wholesalerId, Name = "Test Wholesaler" });

			_mockUnitOfWork.Setup(uow => uow.Beers.GetByIdAsync(beerId))
				.ReturnsAsync(new Beer { Id = beerId, Name = "Test Beer", Price = 5m });

			_mockUnitOfWork.Setup(uow => uow.WholesalerStocks.FindAsync(It.IsAny<Expression<Func<WholesalerStock, bool>>>()))
				.ReturnsAsync(new List<WholesalerStock>
				{
			new WholesalerStock { WholesalerId = wholesalerId, BeerId = beerId, Quantity = stockQuantity }
				});
		}
	}
}
