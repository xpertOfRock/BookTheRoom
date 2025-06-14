﻿using Core.Abstractions;

namespace Core.Entities
{
    public class ChatMessage : IEntity
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string ConnectionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}