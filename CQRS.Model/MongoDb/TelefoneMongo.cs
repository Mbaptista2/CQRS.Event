using CQRS.Model.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Model.MongoDb
{
    public class TelefoneMongo
    {
		[BsonElement("Type")]
		public TipoTelefone Type { get; set; }
		[BsonElement("AreaCode")]
		public int AreaCode { get; set; }
		[BsonElement("Number")]
		public int Number { get; set; }
	}
}
