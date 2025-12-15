using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResult
{
    public class Error
    {

        public string Code { get; }
        public string Description { get;  }
        public ErrorType type { get;  }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            this.type = type;
        }

        //Static Factory Methods  To Create Errors
        public static Error Failure(string code = "General.Failure", string description = "General Failure Has Occurred")
        {
            return new Error(code, description, ErrorType.Failure);
        }
        
        public static Error NotFound(string code = "General.NotFound", string description = "The Request Resource Was Not Found")
        {
            return new Error(code, description, ErrorType.NotFound);
        }
        public static Error UnAuthorized(string code = "General.UnAuthorized", string description = "You Are Not Authorized")
        {
            return new Error(code, description, ErrorType.Unauthorized);
        }
        public static Error Validation(string code = "General.Validation", string description = "Validation Error Has Occurred")
        {
            return new Error(code, description, ErrorType.Validation);
        }
        public static Error Forbidden(string code = "General.Forbidden", string description = "You do not have permission to access this resource")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }
        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "Authentication failed due to invalid credentials.")
        {
            return new Error(code, description, ErrorType.InvalidCredentials);
        }
    }
}
