using CQRS.Domain.Commands;
using CQRS.Model.MongoDb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Domain.Services
{

    public interface IClienteService
    {
      
        Task<bool> IssueCommandAsync(Command cmd);

       
        Task<List<ClienteMongo>> GetAllCustomersAsync();

        Task<ClienteMongo> GetCustomerAsync(Guid custId);

       
        Task<List<ClienteMongo>> GetCustomersByEmailAsync(string email);
    }
}
