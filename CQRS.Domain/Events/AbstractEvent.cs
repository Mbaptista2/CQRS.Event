using CQRS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Events
{
    public class AbstractEvent : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
