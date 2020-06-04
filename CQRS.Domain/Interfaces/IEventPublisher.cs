using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Interfaces
{
    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : IEvent;
    }
}
