using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using Services.FinnhubService;
using Services.StocksService;
using Stock_App;
using Stock_App.Filters.ActionFilters;
using Stock_App.Middleware;
using StockApp_UI.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {
	loggerConfiguration
	.ReadFrom.Configuration(context.Configuration)
	.ReadFrom.Services(services);
});

builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();

app.UseSerilogRequestLogging();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseMiddleware<ExceptionHandlingMiddleware>();
}


if (builder.Environment.IsEnvironment("Test") is false)
	Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
public partial class Program { }