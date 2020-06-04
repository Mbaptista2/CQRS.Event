using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Factories
{
    public static class AggregateFactory
    {
        public static T CreateAggregate<T>()
        {
            try
            {
                return (T)Activator.CreateInstance(typeof(T), true);
            }
            catch (MissingMethodException)
            {
                throw new Exception(typeof(T).ToString());
            }
        }
    }
}
