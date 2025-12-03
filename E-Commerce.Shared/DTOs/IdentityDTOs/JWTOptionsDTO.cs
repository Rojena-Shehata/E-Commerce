using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.IdentityDTOs
{
    public class JWTOptionsDTO
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SecurityKey { get; set; } = default!;
        public double DurationInDays { get; set; } 
    }
}
