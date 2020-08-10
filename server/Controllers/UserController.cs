using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using server.Helpers;
using server.Hubs;
using server.Models;
using server.Services;

namespace server.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private IUserServices _userServices;
        public UserController(IUserServices userServices, IHubContext<ChatHub> chatHub)
        {
            _hubContext = chatHub;
            _userServices = userServices;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Login([FromBody]UserModel model)
        {
            var user = _userServices.Authenticate(model.Email, model.Password);

            if(user == null)
                return BadRequest(new { message = "Username or Password is Incorrect"});
                
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<UserModel> Register([FromBody]UserModel model)
        {
            var user = _userServices.Register(model);
            await _hubContext.Clients.All.SendAsync("NewUser", "Hello New User");
            // create user
            return user;
        }
    }
}
