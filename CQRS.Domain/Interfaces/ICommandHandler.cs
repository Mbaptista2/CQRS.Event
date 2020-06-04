using CQRS.Domain.Interfaces;
using CQRS.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Commands
{
	public interface ICommandHandler<T> : IHandler<T> where T : ICommand
	{
	}
}
