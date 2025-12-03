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
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var result=await _authenticationService.LoginAsync(loginDTO);
            return HandleResult<UserDTO>(result);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var result=await _authenticationService.RegisterAsync(registerDTO);
            return HandleResult<UserDTO>(result);
        }
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            var DoesEmailExist=await _authenticationService.CheckEmailAsync(email);
            return Ok(DoesEmailExist);
        }

        [HttpGet("CurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var result=await _authenticationService.GetUserByEmailAsync(email);
            return HandleResult<UserDTO>(result);
        }

    }
}
