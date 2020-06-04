using CQRS.Domain.Aggregates;
using CQRS.Domain.Interfaces;
using CQRS.Model.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Domain.Commands
{
    public class ClienteCommandHandler: ICommandHandler<CriarClienteCommand>,
                                        ICommandHandler<AtualizarClienteCommand>,
                                        ICommandHandler<ExcluirClienteCommand>
    {
        private readonly ISession _session;

        public ClienteCommandHandler(ISession session)
        {
            _session = session;
        }

        private NLog.Logger logger = NLog.LogManager.GetLogger("CustomerCommandHandlers");
       
        public void Handle(CriarClienteCommand command)
        {
            var item = new ClienteAggregate(
                command.Id,
                command.Email,
                command.Nome,
                command.Idade,
                command.Telefones.Select(x => new TelefoneMongo()
                {
                    Type = x.Type,
                    AreaCode = x.AreaCode,
                    Number = x.Number
                }).ToList(),
                command.ExpectedVersion);
            _session.Add(item);
            _session.Commit();
        }
        private T Get<T>(Guid id, int? expectedVersion = null) where T : AggregateRoot
        {
            try
            {
                return _session.Get<T>(id, expectedVersion);
            }
            catch (Exception e)
            {
                logger.Error("Cannot get object of type {0} with id={1} ({2}) from session", typeof(T), id, expectedVersion);
                throw e;
            }
        }
        public void Handle(AtualizarClienteCommand command)
        {
            logger.Info("Handling UpdateCustomerCommand {0} ({1})", command.Id, command.ExpectedVersion);
            ClienteAggregate item = Get<ClienteAggregate>(command.Id);
            item.Update(
                command.Id,
                command.Nome,
                command.Idade,
                command.Telefones.Select(x => new TelefoneMongo()
                {
                    Type = x.Type,
                    AreaCode = x.AreaCode,
                    Number = x.Number
                }).ToList(),
                command.ExpectedVersion);
            _session.Commit();
        }
        public void Handle(ExcluirClienteCommand command)
        {
            ClienteAggregate item = Get<ClienteAggregate>(command.Id);
            item.Delete();
            _session.Commit();
        }
    }
}
