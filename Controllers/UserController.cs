using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models.DTOs;
using Simplifly.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Simplifly.Models.DTO_s;
using System.Diagnostics.CodeAnalysis;


namespace Simplifly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> RegisterCustomer(RegisterCustomerUserDTO user)
        {
            try
            {
                var result = await _userService.RegisterCustomer(user);
                return Ok(result);
            }catch (UserAlreadyPresentException uape)
            {
                _logger.LogError(uape.Message);
                return BadRequest(uape.Message);
            }
            
        }
        [Route("RegisterFlightOwner")]
        [HttpPost]
        [EnableCors("ReactPolicy")]
        public async Task<LoginUserDTO> RegisterFlightOwner(RegisterFlightOwnerUserDTO user)
        {
            var result = await _userService.RegisterFlightOwner(user);
            return result;
        }
        [Route("RegisterAdmin")]
        [HttpPost]
        public async Task<LoginUserDTO> RegisterAdmin(RegisterAdminUserDTO user)
        {
            var result = await _userService.RegisterAdmin(user);
            return result;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> Login(LoginUserDTO user)
        {
            try
            {
                var result = await _userService.Login(user);
                return Ok(result);
            }
            catch (InvlidUuserException iuse)
            {
                _logger.LogCritical(iuse.Message);
                return Unauthorized("Invalid username or password");
            }

        }
        [Route("UpdatePassword")]
        [HttpPut]
        public async Task<ActionResult<LoginUserDTO>> UpdatePassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            try
            {
                var result = await _userService.UpdateUserPassword(forgotPasswordDTO);
                return Ok(result);
            }
            catch (NoSuchUserException nsue)
            {
                _logger.LogCritical(nsue.Message);
                return Unauthorized("Invalid username");
            }
        }
    }
}
