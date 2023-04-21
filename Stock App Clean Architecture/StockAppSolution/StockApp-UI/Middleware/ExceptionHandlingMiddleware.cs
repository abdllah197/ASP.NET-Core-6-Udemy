using Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace Stock_App.Middleware
{
    public class ExceptionHandlingMiddleware:IMiddleware
    {
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly ILogger<Exception> _logger;

        public ExceptionHandlingMiddleware(IDiagnosticContext diagnosticContext, ILogger<Exception> logger)
        {
			_diagnosticContext = diagnosticContext;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch(FinnhubException ex)
            {
                LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
				LogException(ex);		
                throw;
            }
        }

        public void LogException(Exception ex)
        {
			if (ex.InnerException != null)
			{
				if (ex.InnerException.InnerException != null)
				{
					_logger.LogError("{ExceptionType} {ExceptionMessage}", ex.InnerException.InnerException.GetType().ToString(), ex.InnerException.InnerException.Message);

					_diagnosticContext.Set("Exception", $"{ex.InnerException.InnerException.GetType().ToString()}, {ex.InnerException.InnerException.Message}, {ex.InnerException.InnerException.StackTrace}");
				}
				else
				{
					_logger.LogError("{ExceptionType} {ExceptionMessage}", ex.InnerException.GetType().ToString(), ex.InnerException.Message);

					_diagnosticContext.Set("Exception", $"{ex.InnerException.GetType().ToString()}, {ex.InnerException.Message}, {ex.InnerException.StackTrace}");
				}
			}
			else
			{
				_logger.LogError("{ExceptionType} {ExceptionMessage}", ex.GetType().ToString(), ex.Message);

				_diagnosticContext.Set("Exception", $"{ex.GetType().ToString()}, {ex.Message}, {ex.StackTrace}");
			}
		}
	}

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
