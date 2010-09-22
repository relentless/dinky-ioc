using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Dinky {
    public class Container {

        protected Dictionary<Type, Func<object>> _dependencies = new Dictionary<Type, Func<object>>();

        public bool AllowDynamicResolveFromLoadedAssemblies { get; set; }

        public Container() {
        }

        public ContainerResolutionType Map<T>() {
            return new ContainerResolutionType(this, typeof(T));
        }

        internal void AddDepencency(Type Type, Func<object> Dependency) {
            _dependencies.Add(Type, Dependency);
        }

        public T Resolve<T>(){
            Func<object> dependency = null;
            if (_dependencies.TryGetValue(typeof(T), out dependency)){
                return (T)dependency.Invoke();
            }
            else if (AllowDynamicResolveFromLoadedAssemblies){
                foreach (Type typeImplementingInterface in TypesImplementingInterface(typeof(T))) {
                    if (typeImplementingInterface.GetConstructor(Type.EmptyTypes) != null) {
                        return (T)Activator.CreateInstance(typeImplementingInterface);
                    }
                }
            }
            throw new Exception(string.Format("Dinky cannot resolve type {0}", typeof(T)));
        }

        public static IEnumerable<Type> TypesImplementingInterface(Type desiredType) {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => desiredType.IsAssignableFrom(type));
        }

        public bool CanResolve<T>() {
            if (_dependencies.Keys.Contains(typeof(T))) {
                return true;
            }

            if (AllowDynamicResolveFromLoadedAssemblies) {
                if (TypesImplementingInterface(typeof(T)).Count() > 0) {
                    return true;
                }
            }

            return false;
        }
    }

    public class ContainerResolutionType {
        private Container _container;
        private Type _type;

        internal ContainerResolutionType(Container Container, Type Type) {
            _container = Container;
            _type = Type;
        }

        public void ToThis(object dependency) {
            _container.AddDepencency(_type, delegate { return dependency; });
        }

        public void To<T>() {
            _container.AddDepencency(_type, delegate { return Activator.CreateInstance<T>(); });
        }
    }
}
