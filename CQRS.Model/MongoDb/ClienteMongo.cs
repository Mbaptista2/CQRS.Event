using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Model.MongoDb
{
    public class ClienteMongo
    {
		[BsonElement("Id")]
		public Guid Id { get; set; }
		[BsonElement("Email")]
		public string Email { get; set; }
		[BsonElement("Nome")]
		public string Nome { get; set; }
		[BsonElement("Idade")]
		public int Idade { get; set; }
		[BsonElement("Telefones")]
		public List<TelefoneMongo> Telefones { get; set; }
	}
}
