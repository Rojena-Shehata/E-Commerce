using E_Commerce.Shared.CommonResult;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Admin.Dashboard.Controllers
{
    public class MvcBaseController : Controller
    {

        protected bool IsValidationError { get; private set; }
        protected void HandleErrors(ModelStateDictionary modelState, IReadOnlyList<Error> errors)
        {
             
            if(errors is not null)
            {
                 if (errors.Count == 1 && errors[0].type!=ErrorType.Validation)
                {
                    IsValidationError = false;
                    HandleSingleNonValidationeError(errors[0]);
                }
                else if (errors.All(error => error.type == ErrorType.Validation))
                {
                    IsValidationError = true;
                    HandleValidationModelErrors(modelState, errors);
                }

            }
                               
            
        }
        protected void HandleSuccessMessage(string message="Success Message")
        {
            TempData["SuccessMessage"]=message;
        }
        private void HandleSingleNonValidationeError(Error error)
        {
            TempData["ErrorMessage"]=error.Description;
        }

        protected void HandleValidationModelErrors(ModelStateDictionary modelState,IReadOnlyList<Error> errors)
        {

            foreach (var error in errors)
            {

                ModelState.AddModelError(error.Code, error.Description);
            }
            
        }
    }
}
