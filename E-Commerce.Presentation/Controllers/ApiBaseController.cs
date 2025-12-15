using E_Commerce.Shared.CommonResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class ApiBaseController:ControllerBase
    {
       
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSucceed)
                return NoContent();
            else
                return HandleProblem(result.Errors);
        }
        protected ActionResult HandleResult<TValue>(Result<TValue> result)
        {
            if(result.IsSucceed)
                return Ok(result.Value);
            else
                return HandleProblem(result.Errors);
        }

        //
        private ActionResult HandleProblem(IReadOnlyList<Error> errors )
        {
            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An UnExpected Error Occurred");
            else if (errors.All(e => e.type == ErrorType.Validation))
                return HandleValidationErrors(errors);
            else
                return HandleSingleErrorProblem(errors[0]);


        }
        private ActionResult HandleSingleErrorProblem(Error error)
        {
            return Problem
                (
                    title: error.Code,
                    detail: error.Description,
                    type: error.type.ToString(),
                    statusCode: GetStatusCodeByErrorType(error.type)
                );
        }

        private int GetStatusCodeByErrorType(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        private ActionResult HandleValidationErrors(IReadOnlyList<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modelStateDictionary);
        }
    }
}
