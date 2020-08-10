   using System;

namespace server.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public string message { get; set; }
        public string from { get; set;}
        public Guid sender { get; set;}
        public DateTime message_time { get; set; }
        public Guid convoId { get; set;}

    }
}