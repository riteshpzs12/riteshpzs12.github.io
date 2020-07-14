using System;
namespace SensorData.ContainerHelper
{
    public class RegisteredObject
    {
        public Type TypeToResolve { get; private set; }
        public Type ConcreteType { get; private set; }
        public object Instance { get; private set; }
        public LifeCycle LifeCycle { get; private set; }

        public RegisteredObject(Type typeToResolve, Type concreteType, LifeCycle lifeCycle)
        {
            TypeToResolve = typeToResolve;
            ConcreteType = concreteType;
            LifeCycle = lifeCycle;
        }

        public void CreateInstance(params object[] args)
        {
            this.Instance = Activator.CreateInstance(this.ConcreteType, args);
        }
    }
}
