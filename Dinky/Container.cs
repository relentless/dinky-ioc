using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diggy {
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
            if (_dependencies.TryGetValue(typeof(T), out dependency)) {
                return (T)dependency.Invoke();
            }
            return default(T);
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
