using CQRS.Domain.Interfaces;
using CQRS.Model.MongoDb;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Domain.Events.Handlers
{
    public class ClienteCriadoEventHandler: IBusEventHandler
    {
        private readonly IClienteMongoDbRepository _readModelRepository;
        private Logger logger = LogManager.GetLogger("CustomerCreatedEventHandler");
        public ClienteCriadoEventHandler(IClienteMongoDbRepository readModelRepository)
        {
            this._readModelRepository = readModelRepository;
        }
        public Type HandlerType
        {
            get { return typeof(ClienteCriadoEvent); }
        }
        public async void Handle(IEvent @event)
        {
            ClienteCriadoEvent customerCreatedEvent = (ClienteCriadoEvent)@event;

            await _readModelRepository.Create(new ClienteMongo()
            {
                Id = customerCreatedEvent.Id,
                Email = customerCreatedEvent.Email,
                Nome = customerCreatedEvent.Nome,
                Idade = customerCreatedEvent.Idade,
                Telefones = customerCreatedEvent.Telefones.Select(x =>
                    new TelefoneMongo()
                    {
                        Type = x.Type,
                        AreaCode = x.AreaCode,
                        Number = x.Number
                    }).ToList()
            });
            logger.Info("A new CustomerCreatedEvent has been processed: {0} ({1})", customerCreatedEvent.Id, customerCreatedEvent.Version);
        }
    }
}
