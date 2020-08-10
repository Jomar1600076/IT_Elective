using System;

namespace server.Models
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid fUser { get; set; }
        public Guid sUser { get; set; }
    }
}