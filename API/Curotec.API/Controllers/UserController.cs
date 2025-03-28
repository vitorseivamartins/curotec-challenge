using Curotec.Application.DTOs;
using Curotec.Application.Services;
using Curotec.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Curotec.API.Controllers
{
    [ApiController()]
    [Route("api/user")]
    public sealed class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IUserService userService)
            => _userService = userService;        

        [HttpPost("create")]
        public async Task<ActionResult<User>> CreateUser(UserDto request, CancellationToken cancellationToken)
        {
            return await _userService.CreateUser(request, cancellationToken);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(UserDto request, CancellationToken cancellationToken)
        {
            var response = await _userService.LoginUser(request, cancellationToken);
            return response;
        }
    }
}