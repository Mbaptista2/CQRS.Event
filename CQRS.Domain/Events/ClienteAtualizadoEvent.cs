using CQRS.Domain.Interfaces;
using CQRS.Model.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Domain.Events
{
    public class ClienteAtualizadoEvent : AbstractEvent
    {        
        public string Nome { get; set; }
        public int Idade { get; set; }
        public List<TelefoneMongo> Telefones { get; set; }
		public ClienteAtualizadoEvent()
		{
		}

		public ClienteAtualizadoEvent(Guid id, string name, int age, List<TelefoneMongo> phones, int version)
		{
			Id = id;
			Nome = name;
			Idade = age;
			Telefones = phones;
			Version = version;
		}

	}
}
