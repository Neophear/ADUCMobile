using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ADUCMobile.ADUCBackendService.Models;
using ADUCMobile.ADUCBackendService.Services;
using System.Security.Claims;
using ADUCBackendService.Models.Exceptions;

namespace ADUCMobile.ADUCBackendService.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/User")]
    public class UsersController : ControllerBase
    {
        private IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]User userParam)
        {
            try
            {
                return Ok(await userService.Authenticate(userParam.Username, userParam.Password));
            }
            catch (NotAuthenticatedException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (NotAuthorizedException e)
            {
                return Unauthorized(new { message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong with the service" });
            }
        }

        [HttpGet("authenticateToken")]
        public async Task<IActionResult> AuthenticateToken()
        {
            try
            {
                return Ok(await userService.AuthenticateToken(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            }
            catch (NotAuthenticatedException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (NotAuthorizedException e)
            {
                return Unauthorized(new { message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong with the service" });
            }
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<IActionResult> Get(string username)
        {
            User user = await userService.GetUser(username);

            if (user != null)
                return Ok(user);
            else
                return NotFound($"User {username} not found");
        }
    }
}