using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
	public class StocksRepository : IStocksRepository
	{
		private readonly ApplicationDbContext applicationDbContext;
		public StocksRepository(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
		{
			applicationDbContext.BuyOrders.Add(buyOrder);
			await applicationDbContext.SaveChangesAsync();
			return buyOrder;
		}

		public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
		{
			applicationDbContext.SellOrders.Add(sellOrder);
			await applicationDbContext.SaveChangesAsync();
			return sellOrder;
		}

		public async Task<List<BuyOrder>> GetBuyOrders()
		{
			List<BuyOrder> buyOrders = await applicationDbContext.BuyOrders
																.OrderByDescending(t => t.DateAndTimeOfOrder)
																.ToListAsync();
			return buyOrders;
		}


		public async Task<List<SellOrder>> GetSellOrders()
		{
			List<SellOrder> sellOrders = await applicationDbContext.SellOrders
																.OrderByDescending(t => t.DateAndTimeOfOrder)
																.ToListAsync();
			return sellOrders;
		}
	}
}