using CQRS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Events
{
    public class ClienteExcluidoEvent: AbstractEvent
    {
        public ClienteExcluidoEvent(Guid id, int version)
        {
            Id = id;
            Version = version;
        }
    }
}
