using CQRS.Domain.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Events.Handlers
{
    public class ClienteExcluidoEventHandler: IBusEventHandler
    {
        private readonly IClienteMongoDbRepository readModelRepository;

        private Logger logger = LogManager.GetLogger("CustomerDeletedEventHandler");

        public ClienteExcluidoEventHandler(IClienteMongoDbRepository readModelRepository)
        {
            this.readModelRepository = readModelRepository;
        }

        public Type HandlerType
        {
            get { return typeof(ClienteExcluidoEvent); }
        }

        public async void Handle(IEvent @event)
        {
            ClienteExcluidoEvent customerDeletedEvent = (ClienteExcluidoEvent)@event;

            await readModelRepository.Remove(customerDeletedEvent.Id);

            logger.Info("A new CustomerDeletedEvent has been processed: {0} ({1})", customerDeletedEvent.Id, customerDeletedEvent.Version);
        }
    }
}
 