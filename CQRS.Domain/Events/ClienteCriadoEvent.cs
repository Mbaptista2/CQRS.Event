using CQRS.Domain.Interfaces;
using CQRS.Model.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Domain.Events
{
    public class ClienteCriadoEvent : AbstractEvent
    {        
        public string Email { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public List<TelefoneMongo> Telefones { get; set; }
       
        public ClienteCriadoEvent()
        {
        }

        public ClienteCriadoEvent(Guid id, string email, string name, int age, List<TelefoneMongo> phones, int version)
        {
            Id = id;
            Email = email;
            Nome = name;
            Idade = age;
            Telefones = phones;
            Version = version;
        }

    }
}
