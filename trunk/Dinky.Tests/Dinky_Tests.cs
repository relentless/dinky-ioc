using System;
using System.Collections.Generic;
using NUnit.Framework;
using Dinky;

namespace Dinky.Tests {
    
    [TestFixture]
    public class Dinky_Tests {
        [TearDown]
        public void TearDown() {
            Dinky.ClearMappings();
            Dinky.AllowDynamicResolveFromLoadedAssemblies = false;
        }

        [Test]
        public void DependencyMappedToExistingInstance_ReturnsExistingInstance() {
            // arrange
            Dependency specificDependency = new Dependency();

            // act
            Dinky.Map<IDependency>().ToThis(specificDependency);
            var result = Dinky.Resolve<IDependency>();

            //assert
            Assert.AreEqual(specificDependency, result);
        }

        [Test]
        public void DependencyMappedToExistingInstance_CalledTwice_ReturnsSameInstance() {
            // arrange
            Dependency specificDependency = new Dependency();

            // act
            Dinky.Map<IDependency>().ToThis(specificDependency);
            var result1 = Dinky.Resolve<IDependency>();
            var result2 = Dinky.Resolve<IDependency>();

            //assert
            Assert.AreSame(result1, result2);
        }

        [Test]
        public void DependencyMappedToNewInstance_CalledTwice_ReturnsTwoDifferentNewInstances() {
            // arrange

            // act
            Dinky.Map<IDependency>().To<Dependency>();
            var result1 = Dinky.Resolve<IDependency>();
            var result2 = Dinky.Resolve<IDependency>();

            //assert
            Assert.AreNotEqual(result1, result2);
        }

        [Test]
        public void ResolvedClassHasDependency_ContainerKnowsHowToResolveDependency_ReturnsClassWithDependencyResolved() {
            // arrange

            // act
            Dinky.Map<IDependant>().To<Dependant>();
            var result = Dinky.Resolve<IDependant>();

            //assert
            //Assert.AreEqual(injectedDependency, result);
        }

        [Test]
        public void ResolveTypeFromLoadedAssembly_NoMappingDefined_AllowDynamicResolveSetTrue_TypeAvailableInLoadedAssembly_CreatesNewInstance()
        {
            // arrange
            Dinky.AllowDynamicResolveFromLoadedAssemblies = true;

            // act
            var result = Dinky.Resolve<IDependency>();

            //assert
            Assert.IsTrue(result is IDependency);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void ResolveTypeFromLoadedAssembly_NoMappingDefined_AllowDynamicResolveSetTrue_TypeNotAvailableInLoadedAssembly_ThrowsException() {
            // arrange
            Dinky.AllowDynamicResolveFromLoadedAssemblies = true;

            // act
            var result = Dinky.Resolve<INoImplementation>();

            // (exception)
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void ResolveTypeFromLoadedAssembly_NoMappingDefined_AllowDynamicResolveLeftFalse_ThrowsException() {
            // arrange

            // act
            IDependency result = Dinky.Resolve<IDependency>();

            // (exception)
        }

        [Test]
        public void CanResolve_MappingDefined_ReturnsTrue() {
            // arrange
            Dinky.Map<IDependency>().To<Dependency>();

            // act
            bool result = Dinky.CanResolve<IDependency>();

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanResolve_NoMappingDefined_AllowDynamicResolveLeftFalse_ReturnsFalse() {
            // arrange

            // act
            bool result = Dinky.CanResolve<IDependency>();

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CanResolve_NoMappingDefined_AllowDynamicResolveSetTrue_TypeAvailableInLoadedAssembly_ReturnsTrue() {
            // arrange
            Dinky.AllowDynamicResolveFromLoadedAssemblies = true;

            // act
            bool result = Dinky.CanResolve<IDependency>();

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanResolve_NoMappingDefined_AllowDynamicResolveSetTrue_TypeNotAvailableInLoadedAssembly_ReturnsFalse() {
            // arrange
            Dinky.AllowDynamicResolveFromLoadedAssemblies = true;

            // act
            bool result = Dinky.CanResolve<INoImplementation>();

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
