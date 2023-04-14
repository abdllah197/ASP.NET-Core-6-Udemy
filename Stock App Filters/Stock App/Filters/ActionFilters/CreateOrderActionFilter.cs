using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using Stock_App.Controllers;
using Stock_App.Models;

namespace Stock_App.Filters.ActionFilters
{
    public class CreateOrderActionFactoryFilter : Attribute,IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter=serviceProvider.GetRequiredService<CreateOrderActionFilter>();
            return filter;
        }
    }
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public CreateOrderActionFilter()
        {

        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Before
            if(context.Controller is TradeController tradeController) 
            { 
                var orderRequest= context.ActionArguments["orderRequest"] as IOrderRequest;
                if(orderRequest != null)
                {
                    orderRequest.DateAndTimeOfOrder = DateTime.Now;
                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);
                    if(!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(e => e.Errors).Select(x => x.ErrorMessage).ToList();
                        StockTrade stockTrade = new StockTrade() { StockName = orderRequest.StockName, Price = orderRequest.Price, Quantity = orderRequest.Quantity, StockSymbol = orderRequest.StockSymbol };
                        context.Result= tradeController.View("Index", stockTrade);
                    }
                    else
                        await next();
                }
                else
                   await next();
            }
            else
                await next();
            
            
        }
    }
}
