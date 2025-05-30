﻿using Core.Abstractions;
using Core.ValueObjects;

namespace Core.Entities
{
    public class Hotel : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public bool HasPool { get; set; }
        public List<string>? Images { get; set; }
        
        public Address Address { get; set; }
        public List<Room>? Rooms { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Service>? Services { get; set; }
    }
}
