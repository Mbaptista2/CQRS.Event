using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Interfaces
{
    public interface IBusEventHandler
    {
        Type HandlerType { get; }
        void Handle(IEvent @event);
    }
}
