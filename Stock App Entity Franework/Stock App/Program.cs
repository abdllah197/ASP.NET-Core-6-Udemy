using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Stock_App;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddSingleton<IFinnhubService,FinnhubService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
