using System;
namespace SensorData.ContainerHelper
{
    public interface IIoCContainer
    {
        void Register<TResolve, TConcrete>(LifeCycle lifeCycle);
        void Register<TResolve, TConcrete>();
        TResolve Resolve<TResolve>();
        object Resolve(Type resolve);
    }

    public enum LifeCycle
    {
        Singleton,
        Transient
    }
}
