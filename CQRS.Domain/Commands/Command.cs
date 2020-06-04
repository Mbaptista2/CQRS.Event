using CQRS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CQRS.Domain.Commands
{
    [DataContract]
    [KnownType(typeof(CriarClienteCommand))]
    [KnownType(typeof(AtualizarClienteCommand))]
    [KnownType(typeof(ExcluirClienteCommand))]
    public abstract class Command : ICommand
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public int ExpectedVersion { get; set; }
    }
}
