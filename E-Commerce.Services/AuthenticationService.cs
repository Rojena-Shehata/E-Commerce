using AutoMapper;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<JWTOptionsDTO> _options;
        private readonly IMapper _mapper;

        public AuthenticationService(UserManager<ApplicationUser> userManager,IOptions<JWTOptionsDTO> options,IMapper mapper)
        {
            _userManager = userManager;
            _options = options;
            _mapper = mapper;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var user=await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public async Task<Result<AddressDTO>> CreateOrUpdateUserAddressAsync(AddressDTO addressDTO, string email)
        {
            var user = await _userManager.Users.Where(user => user.Email.ToLower() == email.ToLower()).Include(e => e.Address).FirstOrDefaultAsync();
            if (user is null)
                return Error.NotFound("User.NotFound", $"User With Email:{email} Is Not Found");
            if(user.Address is null)
            {
                user.Address = _mapper.Map<Address>(addressDTO);
            }
            else 
            {
                user.Address.FirstName = addressDTO.FirstName;
                user.Address.LastName = addressDTO.LastName;
                user.Address.Street = addressDTO.Street;
                user.Address.City = addressDTO.City;
                user.Address.Country= addressDTO.Country;
            }
          var res= await _userManager.UpdateAsync(user);
            if(res.Succeeded) 
                return _mapper.Map<AddressDTO>(user.Address);
            else
            {
                return res.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
            }
        }

        public async Task<Result<AddressDTO>> GetCurrentUserAddressByEmailAsync(string email)
        {
            var user =await _userManager.Users.Where(user=>user.Email.ToLower()==email.ToLower()).Include(e=>e.Address).FirstOrDefaultAsync();
            if(user is null)
                return Error.NotFound("User.NotFound", $"User With Email:{email} Is Not Found");
            if(user.Address is null)
                return Error.NotFound("Address.NotFound", $"User With Email:{email} Does Not Have An Address");
            return _mapper.Map<Address, AddressDTO>(user.Address);
        }



        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var user=await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound("User.NotFound",$"User With Email:{email} Is Not Found");

            return new UserDTO(user.Email, user.DisplayName, await GenerateTokenAsync(user));
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user=await _userManager.FindByEmailAsync(loginDTO.Email);
            if(user is null)
                return Error.InvalidCredentials("User.InvalidCredentials");
            var isValidPassword=await  _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if(!isValidPassword)
                return Error.InvalidCredentials("User.InvalidCredentials");

            return new UserDTO(user.Email, user.DisplayName,await GenerateTokenAsync(user));

        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName
            };
            var identityResult=await _userManager.CreateAsync(user);
            if (identityResult.Succeeded)
                return new UserDTO(user.Email, user.DisplayName, await GenerateTokenAsync(user));

            return identityResult.Errors.Select(e=>Error.Validation(e.Code,e.Description)).ToList();
        }


        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)
            };
            var roles=await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var JWTOptions = _options.Value;
             
            var signingCredentialsKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTOptions.SecurityKey));
            var token = new JwtSecurityToken
                (
                    issuer:JWTOptions.Issuer,
                    audience: JWTOptions.Audience,
                    claims:userClaims,
                    expires: DateTime.UtcNow.AddDays(JWTOptions.DurationInDays),
                    signingCredentials:new SigningCredentials(signingCredentialsKey,SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
