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
            Dependency injectedDependency = new Dependency();

            // act
            container.map<IDependency>().to(injectedDependency);
            IDependency result = container.resolve<IDependency>();

            //assert
            Assert.AreEqual(injectedDependency, result);
        }

        [Test]
        public void DependencyMappedToExistingInstance_CalledTwice_ReturnsSameInstance() {
            // arrange
            Container container = new Container();
            Dependency injectedDependency = new Dependency();

            // act
            container.map<IDependency>().to(injectedDependency);
            IDependency result1 = container.resolve<IDependency>();
            IDependency result2 = container.resolve<IDependency>();

            //assert
            Assert.AreSame(result1, result2);
        }

        [Test]
        public void DependencyMappedToNewInstance_CalledTwice_ReturnsTwoDifferentNewInstances() {
            // arrange
            Container container = new Container();

            // act
            container.map<IDependency>().toNew<Dependency>();
            IDependency result1 = container.resolve<IDependency>();
            IDependency result2 = container.resolve<IDependency>();

            //assert
            Assert.AreNotEqual(result1, result2);
        }

        [Test]
        public void ResolvedClassHasDependency_ContainerKnowsHowToResolveDependency_ReturnsClassWithDependencyResolved() {
            // arrange
            Container container = new Container();

            // act
            container.map<IDependant>().toNew<Dependant>();
            IDependant result = container.resolve<IDependant>();

            //assert
            //Assert.AreEqual(injectedDependency, result);
        }

        [Test]
        public void ResolveTypeWithParameterlessConstructor_FromLoadedAssembly_NoMappingDefined_CreatesNewInstance()
        {
            // arrange
            Container container = new Container();

            // act
            IDependency result = container.resolve<IDependency>();

            //assert
            Assert.IsTrue(result is IDependency);
        }
    }

    public class Dependency : IDependency { }

    public interface IDependant { }

    public class Dependant: IDependant {
        public Dependant(IDependency dependency) { }
    }

    public interface IDependency {}
}
