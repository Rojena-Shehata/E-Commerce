using E_Commerce.Shared.CommonResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Admin.Dashboard.Controllers
{
    public class MvcBaseController : Controller
    {
        protected void HandleModelErrors(ModelStateDictionary modelState,Result result)
        {
            if (result is not null)
            {
                if (result.Errors.Any())
                    foreach (var error in result.Errors)
                    {
                        
                        ModelState.AddModelError(error.Code, error.Description);
                    }
            }
        }
    }
}
