using CQRS.Domain.Events;
using CQRS.Model;
using CQRS.Model.MongoDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Aggregates
{
    
        public class ClienteAggregate : AggregateRoot
        {
            private string email;
            private string nome;
            private int idade;
            private List<TelefoneMongo> telefones;
            private void Apply(ClienteCriadoEvent e)
            {
                Version = e.Version++;
                email = e.Email;
                idade = e.Idade;
            telefones = e.Telefones;
            }
            private void Apply(ClienteAtualizadoEvent e)
            {
                Version = e.Version++;
                nome = e.Nome;
                idade = e.Idade;
                telefones = e.Telefones;
            }
            private void Apply(ClienteExcluidoEvent e)
            {
                Version = e.Version++;
            }
            private ClienteAggregate() { }
            public ClienteAggregate(Guid id, string email, string name, int age, List<TelefoneMongo> phones, int version)
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("email");
                }
                else if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("name");
                }
                else if (age == 0)
                {
                    throw new ArgumentException("age");
                }
                else if (phones == null || phones.Count == 0)
                {
                    throw new ArgumentException("phones");
                }
                Id = id;
                ApplyChange(new ClienteCriadoEvent(id, email, name, age, phones, version));
            }
            public void Update(Guid id, string name, int age, List<TelefoneMongo> phones, int version)
            {
                ApplyChange(new ClienteAtualizadoEvent(id, name, age, phones, version));
            }
            public void Delete()
            {
                ApplyChange(new ClienteExcluidoEvent(Id, Version));
            }
        
    }
}
