using CQRS.Model.MongoDb;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infra.Data.MongoDB
{
	public class ClienteMongoDbRepository : IClienteMongoDbRepository
	{
		private const string _customerDB = "ClienteDB";
		private const string _customerCollection = "Clientes";
		private IMongoDatabase _db;
		public ClienteMongoDbRepository()
		{
			MongoClient _client = new MongoClient("mongodb://localhost:27017");
			_db = _client.GetDatabase(_customerDB);

			_db.DropCollection(_customerCollection);
			_db.CreateCollection(_customerCollection);
		}

		public Task<List<ClienteMongo>> GetCustomers()
		{
			return Task.Run(() =>
			{
				return _db.GetCollection<ClienteMongo>(_customerCollection).Find(_ => true).ToList();
			});
		}

		public Task<ClienteMongo> GetCustomer(Guid id)
		{
			return Task.Run(() =>
			{
				return _db.GetCollection<ClienteMongo>(_customerCollection).Find(customer => customer.Id.Equals(id)).SingleOrDefault();
			});
		}

		public Task<List<ClienteMongo>> GetCustomerByEmail(string email)
		{
			return Task.Run(() =>
			{
				return _db.GetCollection<ClienteMongo>(_customerCollection).Find(customer => customer.Email == email).ToList();
			});
		}

		public Task<bool> Create(ClienteMongo customer)
		{
			return Task.Run(() =>
			{
				_db.GetCollection<ClienteMongo>(_customerCollection).InsertOne(customer);
				return true;
			});
		}

		public Task<bool> Update(ClienteMongo customer)
		{
			return Task.Run(() =>
			{
				var filter = Builders<ClienteMongo>.Filter.Where(_ => _.Id == customer.Id);

				_db.GetCollection<ClienteMongo>(_customerCollection).ReplaceOne(filter, customer);

				return true;
			});
		}

		public Task<bool> Remove(Guid id)
		{
			return Task.Run(() =>
			{
				var filter = Builders<ClienteMongo>.Filter.Where(_ => _.Id.Equals(id));
				var operation = _db.GetCollection<ClienteMongo>(_customerCollection).DeleteOne(filter);

				return true;
			});
		}
	}
}
