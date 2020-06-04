using CQRS.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Interfaces
{
    public interface ISession
    {
        void Add<T>(T aggregate) where T : AggregateRoot;
        T Get<T>(Guid id, int? expectedVersion = null) where T : AggregateRoot;
        void Commit();
    }
}
