using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Interfaces
{
    public interface IEventStore
    {
        void Save(IEvent @event);
        IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion);
    }
}
