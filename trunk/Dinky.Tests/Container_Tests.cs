using System;
using System.Collections.Generic;
using NUnit.Framework;
using Dinky;

namespace Dinky.Tests {
    
    [TestFixture]
    public class Container_Tests {
        [Test]
        public void DependencyMappedToExistingInstance_ReturnsExistingInstance() {
            // arrange
            Container container = new Container();
            Dependency specificDependency = new Dependency();

            // act
            container.Map<IDependency>().ToThis(specificDependency);
            var result = container.Resolve<IDependency>();

            //assert
            Assert.AreEqual(specificDependency, result);
        }

        [Test]
        public void DependencyMappedToExistingInstance_CalledTwice_ReturnsSameInstance() {
            // arrange
            Container container = new Container();
            Dependency specificDependency = new Dependency();

            // act
            container.Map<IDependency>().ToThis(specificDependency);
            var result1 = container.Resolve<IDependency>();
            var result2 = container.Resolve<IDependency>();

            //assert
            Assert.AreSame(result1, result2);
        }

        [Test]
        public void DependencyMappedToNewInstance_CalledTwice_ReturnsTwoDifferentNewInstances() {
            // arrange
            Container container = new Container();

            // act
            container.Map<IDependency>().To<Dependency>();
            var result1 = container.Resolve<IDependency>();
            var result2 = container.Resolve<IDependency>();

            //assert
            Assert.AreNotEqual(result1, result2);
        }

        [Test]
        public void ResolvedClassHasDependency_ContainerKnowsHowToResolveDependency_ReturnsClassWithDependencyResolved() {
            // arrange
            Container container = new Container();

            // act
            container.Map<IDependant>().To<Dependant>();
            var result = container.Resolve<IDependant>();

            //assert
            //Assert.AreEqual(injectedDependency, result);
        }

        [Test]
        public void ResolveTypeFromLoadedAssembly_NoMappingDefined_AllowDynamicResolveSetTrue_TypeAvailableInLoadedAssembly_CreatesNewInstance()
        {
            // arrange
            Container container = new Container();
            container.AllowDynamicResolveFromLoadedAssemblies = true;

            // act
            var result = container.Resolve<IDependency>();

            //assert
            Assert.IsTrue(result is IDependency);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void ResolveTypeFromLoadedAssembly_NoMappingDefined_AllowDynamicResolveSetTrue_TypeNotAvailableInLoadedAssembly_ThrowsException() {
            // arrange
            Container container = new Container();
            container.AllowDynamicResolveFromLoadedAssemblies = true;

            // act
            var result = container.Resolve<INoImplementation>();

            // (exception)
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void ResolveTypeFromLoadedAssembly_NoMappingDefined_AllowDynamicResolveLeftFalse_ThrowsException() {
            // arrange
            Container container = new Container();

            // act
            IDependency result = container.Resolve<IDependency>();

            // (exception)
        }

        [Test]
        public void CanResolve_MappingDefined_ReturnsTrue() {
            // arrange
            Container container = new Container();
            container.Map<IDependency>().To<Dependency>();

            // act
            bool result = container.CanResolve<IDependency>();

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanResolve_NoMappingDefined_AllowDynamicResolveLeftFalse_ReturnsFalse() {
            // arrange
            Container container = new Container();

            // act
            bool result = container.CanResolve<IDependency>();

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CanResolve_NoMappingDefined_AllowDynamicResolveSetTrue_TypeAvailableInLoadedAssembly_ReturnsTrue() {
            // arrange
            Container container = new Container();
            container.AllowDynamicResolveFromLoadedAssemblies = true;

            // act
            bool result = container.CanResolve<IDependency>();

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanResolve_NoMappingDefined_AllowDynamicResolveSetTrue_TypeNotAvailableInLoadedAssembly_ReturnsFalse() {
            // arrange
            Container container = new Container();
            container.AllowDynamicResolveFromLoadedAssemblies = true;

            // act
            bool result = container.CanResolve<INoImplementation>();

            //assert
            Assert.IsFalse(result);
        }
    }

    public class Dependency : IDependency { }

    public interface IDependant { }

    public interface INoImplementation { }

    public class Dependant: IDependant {
        public Dependant(IDependency dependency) { }
    }

    public interface IDependency {}
}
