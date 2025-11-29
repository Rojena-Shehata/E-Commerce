using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Exceptions.NotFoundExceptions
{
    public sealed class ProductNotFoundException(int Id) : NotFoundException($"Product With {Id} Not Found");

}
