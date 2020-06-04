using CQRS.Domain.Commands;
using CQRS.Model.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Domain.Services
{
   public class ClienteService: IClienteService
    {
        private readonly ClienteCommandHandler _commandHandlers;
        private readonly IClienteMongoDbRepository _readModelRepository;

        public ClienteService(ClienteCommandHandler commandHandlers, IClienteMongoDbRepository readModelRepository)
        {
            this._commandHandlers = commandHandlers;
            this._readModelRepository = readModelRepository;
        }

        public async Task<bool> IssueCommandAsync(Command cmd)
        {
            await Task.Run(() =>
            {
                var method = (from meth in typeof(ClienteCommandHandler)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                              let @params = meth.GetParameters()
                              where @params.Count() == 1 && @params[0].ParameterType == cmd.GetType()
                              select meth).FirstOrDefault();

                if (method == null)
                {
                    var name = cmd.GetType().Name;
                    throw new Exception(string.Format("Command handler of {0} not found", name));
                }

                method.Invoke(_commandHandlers, new[] { cmd });
            });

            return true;
        }

        public async Task<List<ClienteMongo>> GetAllCustomersAsync()
        {
            return await _readModelRepository.GetCustomers();
        }

        public async Task<ClienteMongo> GetCustomerAsync(Guid orderId)
        {
            return await _readModelRepository.GetCustomer(orderId);
        }

        public async Task<List<ClienteMongo>> GetCustomersByEmailAsync(string email)
        {
            return await _readModelRepository.GetCustomerByEmail(email);
        }
    }
}
