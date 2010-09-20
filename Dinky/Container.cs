using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Dinky {
    public class Container {

        protected Dictionary<Type, Func<object>> _dependencies = new Dictionary<Type, Func<object>>();

        public Container() {
        }

        public ContainerResolutionType map<T>() {
            return new ContainerResolutionType(this, typeof(T));
        }

        internal void AddDepencency(Type Type, Func<object> Dependency) {
            _dependencies.Add(Type, Dependency);
        }

        public T resolve<T>(){
            Func<object> dependency = null;
            if (_dependencies.TryGetValue(typeof(T), out dependency)){
                return (T)dependency.Invoke();
            }
            else{
                foreach (Type typeImplementingInterface in TypesImplementingInterface(typeof(T))) {
                    if (typeImplementingInterface.GetConstructor(Type.EmptyTypes) != null) {
                        return (T)Activator.CreateInstance(typeImplementingInterface);
                    }
                }
            }
            return default(T);
        }

        public static IEnumerable<Type> TypesImplementingInterface(Type desiredType) {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => desiredType.IsAssignableFrom(type) &&
                    IsRealClass(type));

        }

        public static bool IsRealClass(Type testType) {
            return testType.IsAbstract == false
                && testType.IsGenericTypeDefinition == false
                && testType.IsInterface == false;
        }
    }

    public class ContainerResolutionType {
        private Container _container;
        private Type _type;

        internal ContainerResolutionType(Container Container, Type Type) {
            _container = Container;
            _type = Type;
        }

        public void to(object dependency) {
            _container.AddDepencency(_type, delegate { return dependency; });
        }

        public void toNew<T>() {
            _container.AddDepencency(_type, delegate { return Activator.CreateInstance<T>(); });
        }
    }
}
