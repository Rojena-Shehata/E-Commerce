using E_Commerce.Services.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.CustomeMiddleWares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate next, ILogger<ExceptionHandlerMiddleWare> logger) 
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    var response = new ProblemDetails()
                    {
                        Title = "Error While Processing The HTTP Request - EndPoint Not Found",
                        Detail=$"EndPoint {httpContext.Request.Path} Not Found",
                        Status=StatusCodes.Status404NotFound,
                        Instance=httpContext.Request.Path
                    };

                    await httpContext.Response.WriteAsJsonAsync(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong:");
                var proplem = new ProblemDetails()
                {
                    Title= "Error While Processing:",
                    Detail=ex.Message,
                    Instance=httpContext.Request.Path,
                    Status = ex switch
                    {
                        NotFoundException=>StatusCodes.Status404NotFound,
                        _=>StatusCodes.Status500InternalServerError
                    }
                };
                httpContext.Response.StatusCode = proplem.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(proplem);
            }
             
        }
    }
}
