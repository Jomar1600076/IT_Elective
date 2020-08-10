using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using server.Models;

namespace server.Hubs
{
    public interface IChatHub
    {   
        Task MessageAdded(MessageModel message);
        Task GetMessages();
    }
    public class ChatHub: Hub<IChatHub>
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("ChatHub hub connected");
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("ChatHub hub disconnected");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task JoinConvo(Guid otherUserId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, otherUserId.ToString());
        }
        public async Task LeaveConvo(Guid otherUserId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, otherUserId.ToString());
        }
    }   
}