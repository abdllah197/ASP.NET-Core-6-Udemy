using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;
using Stock_App;
using Stock_App.Filters.ActionFilters;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((HostBuilderContext context,IServiceProvider services,LoggerConfiguration loggerConfiguration) =>{
	loggerConfiguration
	.ReadFrom.Configuration(context.Configuration)
	.ReadFrom.Services(services);
});

builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddTransient<IStocksService, StocksService>();
builder.Services.AddTransient<IFinnhubService, FinnhubService>();
builder.Services.AddTransient<IStocksRepository, StocksRepository>();
builder.Services.AddTransient<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddTransient<CreateOrderActionFilter>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHttpClient();
var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

if (builder.Environment.IsEnvironment("Test") is false)
	Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
public partial class Program { }