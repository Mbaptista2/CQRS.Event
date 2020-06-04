using CQRS.Domain.Interfaces;
using CQRS.Model.MongoDb;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Domain.Events.Handlers
{
    public class ClienteAtualizadoEventHandler: IBusEventHandler
    {
        private readonly IClienteMongoDbRepository readModelRepository;

        private Logger logger = LogManager.GetLogger("CustomerUpdatedEventHandler");

        public ClienteAtualizadoEventHandler(IClienteMongoDbRepository readModelRepository)
        {
            this.readModelRepository = readModelRepository;
        }

        public Type HandlerType
        {
            get { return typeof(ClienteAtualizadoEvent); }
        }

        public async void Handle(IEvent @event)
        {
            ClienteAtualizadoEvent customerUpdatedEvent = (ClienteAtualizadoEvent)@event;

            ClienteMongo customer = await readModelRepository.GetCustomer(@event.Id);

            await readModelRepository.Update(new ClienteMongo()
            {
                Id = customerUpdatedEvent.Id,
                Email = customer.Email,
                Nome = customerUpdatedEvent.Nome != null ? customerUpdatedEvent.Nome : customer.Nome,
                Idade = customerUpdatedEvent.Idade != 0 ? customerUpdatedEvent.Idade : customer.Idade,
                Telefones = customerUpdatedEvent.Telefones != null ? customerUpdatedEvent.Telefones.Select(x =>
                    new TelefoneMongo()
                    {
                        Type = x.Type,
                        AreaCode = x.AreaCode,
                        Number = x.Number
                    }).ToList() : customer.Telefones
            });

            logger.Info("A new CustomerUpdatedEvent has been processed: {0} ({1})", customerUpdatedEvent.Id, customerUpdatedEvent.Version);
        }
    }
}
