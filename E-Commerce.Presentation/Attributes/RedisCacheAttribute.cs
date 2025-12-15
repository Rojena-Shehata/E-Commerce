using E_Commerce.ServicesAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Attributes
{
    internal class RedisCacheAttribute:ActionFilterAttribute
    {
        private readonly int _durationFromMin;

        public RedisCacheAttribute(int durationFromMin=5)
        {
            _durationFromMin = durationFromMin;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();
           var cacheKey= CreateCacheKey(context.HttpContext.Request);
            var cacheValue =await cacheService.GetAsync(cacheKey);
            if (cacheValue is not null)
            {
                 context.Result = new ContentResult()
                {
                    Content=cacheValue,
                    ContentType="application/json",
                    StatusCode=StatusCodes.Status200OK
                };
                return;
            }

            var actionExcutedContext=await next.Invoke();
            if(actionExcutedContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(_durationFromMin));
            }
        }

        private string CreateCacheKey(HttpRequest httpRequest)
        {
            var cacheKey = new StringBuilder();
            cacheKey.Append(httpRequest.Path);
            foreach(var query  in httpRequest.Query.OrderBy(x=>x.Key))
            {
                cacheKey.Append($"|{query.Key}-{query.Value}");
            }
            return cacheKey.ToString();
        }
    }
}
