using CQRS.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Interfaces
{
    public interface IEvent : IMessage
    {
        Guid Id { get; set; }
        int Version { get; set; }
        DateTimeOffset TimeStamp { get; set; }
    }
}
