using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {

            var errors = actionContext.ModelState.Where(x => x.Value.Errors.Any())
                                                .ToDictionary(x => x.Key, x => x.Value.Errors
                                                                                    .Select(x => x.ErrorMessage).ToList());
            var problem = new ProblemDetails()
            {
                Title = "Validation Error!!",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or More Validation Error Occured!",
                Extensions = { { "Errors", errors } }
            };
            return new BadRequestObjectResult(problem);
        }

    }
}
