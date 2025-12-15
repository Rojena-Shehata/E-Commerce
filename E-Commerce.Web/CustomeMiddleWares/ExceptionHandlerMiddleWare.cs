using E_Commerce.Services.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

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
                if (!httpContext.Response.HasStarted)
                {
                    await MapStatusCodeToResponseAsync(httpContext);
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

        private static async Task MapStatusCodeToResponseAsync(HttpContext httpContext)
        {

            switch (httpContext.Response.StatusCode)
            {
                case StatusCodes.Status401Unauthorized:
                    await HandleResponseAsync(httpContext, "You Are Not Authorized", StatusCodes.Status401Unauthorized);
                    break;
                case StatusCodes.Status404NotFound:
                    await HandleResponseAsync(httpContext, $"EndPoint {httpContext.Request.Path} Not Found", StatusCodes.Status404NotFound, "EndPoint Not Found");
                    break;

            };
            
        }
        private static async Task HandleResponseAsync(HttpContext httpContext,string detail,int statusCode,string title="")
        {
            var response = new ProblemDetails()
            {

                Title = $"Error While Processing The HTTP Request..{title}",
                Detail = $"{detail}",
                Status = statusCode,
                Instance = httpContext.Request.Path
            };
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}
