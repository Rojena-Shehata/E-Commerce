using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.IdentityDTOs
{
    public record AddressDTO
        (
            string FirstName,
            string LastName,
            string Street,
            string City,
            string Country
        );
    
    
}
