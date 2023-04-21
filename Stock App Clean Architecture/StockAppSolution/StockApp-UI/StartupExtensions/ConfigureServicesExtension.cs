using Entities;
using Repositories;
using RepositoryContracts;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using Stock_App.Filters.ActionFilters;
using Stock_App.Middleware;
using Stock_App;
using Microsoft.EntityFrameworkCore;
using Services.FinnhubService;
using Services.StocksService;
using Serilog;

namespace StockApp_UI.StartupExtensions
{
	public static class ConfigureServicesExtension
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services,IConfiguration configuration)
		{
			

			services.AddControllersWithViews();
			services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));
			services.AddTransient<IStocksBuyOrderService, StocksBuyOrderService>();
			services.AddTransient<IStocksSellOrderService, StocksSellOrderService>();
			services.AddTransient<IStocksRepository, StocksRepository>();
			services.AddTransient<IFinnhubGetService, FinnhubGetService>();
			services.AddTransient<IFinnhubSearchService, FinnhubSearchService>();
			services.AddTransient<IFinnhubRepository, FinnhubRepository>();
			services.AddTransient<CreateOrderActionFilter>();

			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
			});

			services.AddHttpLogging(options =>
			{
				options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
			});

			services.AddHttpClient();
			services.AddTransient<ExceptionHandlingMiddleware>();

			return services;
		}
	}
}
