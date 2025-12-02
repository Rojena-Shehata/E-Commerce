using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResult
{
    public enum ErrorType
    {
        Failure = 0,
        NotFound = 1,
        Unauthorized = 2,
        Validation,
        Forbidden,
        InvalidCredentials
    }
}
