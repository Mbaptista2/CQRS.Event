using CQRS.Model.MongoDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IClienteMongoDbRepository
{
    Task<bool> Create(ClienteMongo customer);
    Task<ClienteMongo> GetCustomer(Guid id);
    Task<List<ClienteMongo>> GetCustomerByEmail(string email);
    Task<List<ClienteMongo>> GetCustomers();
    Task<bool> Remove(Guid id);
    Task<bool> Update(ClienteMongo customer);
}