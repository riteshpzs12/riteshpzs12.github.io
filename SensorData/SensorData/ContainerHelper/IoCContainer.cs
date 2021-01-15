using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Internals;

namespace SensorData.ContainerHelper
{
    [Preserve(AllMembers = true)]
    public class IoCContainer : IIoCContainer
    {
        private IList<RegisteredObject> registeredObjects = new List<RegisteredObject>();

        public void Register<TResolve, TConcrete>(LifeCycle lifeCycle)
        {
            registeredObjects.Add(new RegisteredObject(typeof(TResolve), typeof(TConcrete), lifeCycle));
        }

        public void Register<TResolve, TConcrete>()
        {
            registeredObjects.Add(new RegisteredObject(typeof(TResolve), typeof(TConcrete), LifeCycle.Transient));
        }

        public TResolve Resolve<TResolve>()
        {
            return (TResolve)ResolveObject(typeof(TResolve));
        }

        public object Resolve(Type resolve)
        {
            return ResolveObject(resolve);
        }

        private object ResolveObject(Type type)
        {
            var registeredObject = registeredObjects.FirstOrDefault(o => o.TypeToResolve == type);
            if (registeredObject == null)
            {
                throw new TypeNotRegisteredException(string.Format(
                    "The type {0} has not been registered", type.Name));
            }

            return GetInstance(registeredObject);
        }

        private object GetInstance(RegisteredObject registeredObject)
        {
            if (registeredObject.Instance == null ||
                registeredObject.LifeCycle == LifeCycle.Transient)
            {
                var parameters = ResolveConstructorParameters(registeredObject);
                registeredObject.CreateInstance(parameters.ToArray());
            }

            return registeredObject.Instance;
        }

        private IEnumerable<object> ResolveConstructorParameters(RegisteredObject registeredObject)
        {
            var constructorInfo = registeredObject.ConcreteType.GetConstructors().First();
            foreach (var parameter in constructorInfo.GetParameters())
            {
                yield return ResolveObject(parameter.ParameterType);
            }
        }
    }

    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(string message)
            : base(message)
        {
        }
    }
}
