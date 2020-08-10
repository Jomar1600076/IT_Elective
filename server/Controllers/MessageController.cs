using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using server.Helpers;
using server.Hubs;
using server.Models;

namespace server.Controllers
{
 
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private DataContext _dataContext;
        private readonly IHubContext<ChatHub, IChatHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MessageController(DataContext dataContext, IHttpContextAccessor httpContextAccessor, IHubContext<ChatHub, IChatHub> hubContext)
        {
            _hubContext = hubContext;
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("addmessage/{otherUserId}")]
        public async Task<MessageModel> AddMessage(Guid otherUserId, [FromBody]MessageModel message)
        {
            message.Id = Guid.NewGuid();
            message.convoId = otherUserId;
            DateTime when = DateTime.Now;
            message.message_time = when;
            _dataContext.Message.Add(message);
            _dataContext.SaveChanges();
            
            await _hubContext.Clients.Group(otherUserId.ToString()).MessageAdded(message);
            // create user

            return message;
        }

        [HttpPost("addconvo")]
        public async Task<Conversation> AddConvo([FromBody]Conversation convo)
        {
            var currentUser = new Guid(_httpContextAccessor.HttpContext.User.Identity.Name);
            convo.Id = Guid.NewGuid();
            convo.fUser = currentUser;
            await _dataContext.Convo.AddAsync(convo);
            await _dataContext.SaveChangesAsync();
            // create user
            return convo;
        }

        [HttpGet("getConversation")]
        public IEnumerable getConversation()
        {   
            var convoModel = _dataContext.Convo;
            var userModel = _dataContext.Users;
            var currentUser = new Guid(_httpContextAccessor.HttpContext.User.Identity.Name);
            
            var loadUsers =   from convo in convoModel
                            join user in userModel on convo.sUser equals user.Id
                            where (convo.sUser == currentUser || convo.fUser == currentUser)
                            select new {
                                ConvoId = convo.Id,
                                User = user.Id,
                                FirstName = user.FirstName, 
                                LastName = user.LastName
                             };
                              return loadUsers;
        }

        [HttpGet("getmessage/{otherUserId}")]
        public IEnumerable getMessage(Guid otherUserId)
        {   
            var messModel = _dataContext.Message;
            var userModel = _dataContext.Users;
            var convoModel = _dataContext.Convo;
            var currentUser = new Guid(_httpContextAccessor.HttpContext.User.Identity.Name);

            var loadMes =   from convo in convoModel
                            join mess in messModel on convo.Id equals mess.convoId
                            join user in userModel on mess.sender equals user.Id
                            orderby mess.message_time
                            where( mess.convoId == convo.Id && convo.Id == otherUserId)
                            select new { 
                                id = mess.Id,
                                ConvoId = mess.convoId,
                                message = mess.message,
                                Sender = user.Id,
                                UserId = user.Id,
                                message_time = mess.message_time,
                                SenderName = user.FirstName
                             };
            return loadMes;
        }

        [HttpGet("getotheruser/{otherUserId}")]
        public IActionResult getOtherUser(Guid otherUserId)
        {   
            var Users = from user in _dataContext.Users
                           where user.Id == otherUserId
                           select user;
            return Ok(Users);
        }

        [HttpGet("chatlist")]
        public IEnumerable<UserModel>GetAll(){

            var currentUser = new Guid(_httpContextAccessor.HttpContext.User.Identity.Name);
            var convo = _dataContext.Convo;

            var Users = from user in _dataContext.Users
                           where (user.Id != currentUser )
                           select user;
            return Users;
        }
    }
}
