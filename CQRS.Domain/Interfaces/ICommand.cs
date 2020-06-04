using CQRS.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Interfaces
{
    public interface ICommand : IMessage
    {
        Guid Id { get; set; }
        int ExpectedVersion { get; set; }
    }
}
