using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    
    public class AuthenticationController : ApiBaseController
    {
        private readonly IServiceManager _serviceManager;

        public AuthenticationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var result=await _serviceManager.AuthenticationService.LoginAsync(loginDTO);
            return HandleResult<UserDTO>(result);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var result=await _serviceManager.AuthenticationService.RegisterAsync(registerDTO);
            return HandleResult<UserDTO>(result);
        }
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            var DoesEmailExist=await _serviceManager.AuthenticationService.CheckEmailAsync(email);
            return Ok(DoesEmailExist);
        }

        [HttpGet("CurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var result=await _serviceManager.AuthenticationService.GetUserByEmailAsync(email);
            return HandleResult<UserDTO>(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser02()
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var result=await _serviceManager.AuthenticationService.GetUserByEmailAsync(email);
            return HandleResult<UserDTO>(result);
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address=await _serviceManager.AuthenticationService.GetCurrentUserAddressByEmailAsync(email);
            return HandleResult<AddressDTO>(address);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTO>> UpdateCurrentUserAddress(AddressDTO addressDTO)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address=await _serviceManager.AuthenticationService.CreateOrUpdateUserAddressAsync(addressDTO,email);
            return HandleResult<AddressDTO>(address);
        }

    }
}
