using CQRS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Commands
{
    public class CriarTelefoneCommand : Command
    {
        public TipoTelefone Type { get; set; }
        public int AreaCode { get; set; }
        public int Number { get; set; }
    }
}
