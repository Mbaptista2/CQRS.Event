using CQRS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;

namespace CQRS.Domain.Aggregates
{
    public abstract class AggregateRoot
    {
        private readonly List<IEvent> _changes = new List<IEvent>();

        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            lock (_changes)
            {
                return _changes.ToArray();
            }
        }

        public void MarkChangesAsCommitted()
        {
            lock (_changes)
            {
                Version = Version + _changes.Count;
                _changes.Clear();
            }
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                if (e.Version != Version + 1)
                    throw new Exception(e.Id.ToString());
                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(IEvent @event, bool isNew)
        {
            lock (_changes)
            {
                this.AsDynamic().Apply(@event);
                if (isNew)
                {
                    _changes.Add(@event);
                }
                else
                {
                    Id = @event.Id;
                    Version++;
                }
            }
        }
    }
    internal static class PrivateReflectionDynamicObjectExtensions
    {
        public static dynamic AsDynamic(this object o)
        {
            return PrivateReflectionDynamicObject.WrapObjectIfNeeded(o);
        }
    }
    internal class PrivateReflectionDynamicObject : DynamicObject
    {
        public object RealObject { get; set; }
        private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        internal static object WrapObjectIfNeeded(object o)
        {
            // Don't wrap primitive types, which don't have many interesting internal APIs
            if (o == null || o.GetType().IsPrimitive || o is string)
                return o;

            return new PrivateReflectionDynamicObject() { RealObject = o };
        }

        // Called when a method is called
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = InvokeMemberOnType(RealObject.GetType(), RealObject, binder.Name, args);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = WrapObjectIfNeeded(result);

            return true;
        }

        private static object InvokeMemberOnType(Type type, object target, string name, object[] args)
        {
            try
            {
                // Try to incoke the method
                return type.InvokeMember(
                    name,
                    BindingFlags.InvokeMethod | bindingFlags,
                    null,
                    target,
                    args);
            }
            catch (MissingMethodException)
            {
                // If we couldn't find the method, try on the base class
                if (type.BaseType != null)
                {
                    return InvokeMemberOnType(type.BaseType, target, name, args);
                }
                //Don't care if the method don't exist.
                return null;
            }
        }
    }
}
