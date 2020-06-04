using CQRS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Domain
{
   public class ClienteEventStore: IEventStore
    {
        private readonly Dictionary<Guid, List<IEvent>> customerInMemDictionary = new Dictionary<Guid, List<IEvent>>();

        public IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion)
        {
            List<IEvent> customerEvents;
            customerInMemDictionary.TryGetValue(aggregateId, out customerEvents);
            if (customerEvents != null)
            {
                return customerEvents.Where(x => x.Version > fromVersion);
            }

            return new List<IEvent>();
        }

        public void Save(IEvent @event)
        {
            List<IEvent> customerEvents;
            customerInMemDictionary.TryGetValue(@event.Id, out customerEvents);
            if (customerEvents == null)
            {
                customerEvents = new List<IEvent>();
                customerInMemDictionary.Add(@event.Id, customerEvents);
            }
            customerEvents.Add(@event);
        }
    }
}
