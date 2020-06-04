using CQRS.Domain.Events;
using CQRS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CQRS.Domain.Commands
{
	[DataContract]
    public class AtualizarClienteCommand : Command
    {
		[DataMember]
		public string Nome { get; set; }
		[DataMember]
		public int Idade { get; set; }
		[DataMember]
		public List<CriarTelefoneCommand> Telefones { get; set; }		
	}
}
