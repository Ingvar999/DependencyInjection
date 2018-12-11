using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIContainer;
using System.Linq;

namespace DIContainerTests
{
    [TestClass]
    public class DIContainerTests
    {
        private readonly DependenciesConfiguration config;
        private readonly DependencyProvider provider;

        public DIContainerTests()
        {
            config = new DependenciesConfiguration();
            config.Register<IDependency, Class1>();
            config.Register<IDependency, Class2>();
            config.Register<IDependency, Class11>();
            config.Register<Class1, Class1>();
            config.Register<Class1, Class11>();
            config.Register<IRepository, RepositoryImpl>();
            config.Register<IRepository, MySqlRepository>();
            config.Register(typeof(IService<>), typeof(ServiceImpl<>));
            config.Register<IService<MySqlRepository>, ServiceImpl<MySqlRepository>>();
            config.Register<IDep, DependencyClass>();
            provider = new DependencyProvider(config);
        }

        [TestMethod]
        public void SingletonPolicyTest()
        {
            var sProvider = new DependencyProvider(config, true);
            var obj1 = sProvider.Resolve<IDependency>().First();
            var obj2 = sProvider.Resolve<IDependency>().First();
            Assert.AreSame(obj1, obj2);
        }

        [TestMethod]
        public void InstancePerPolicyTest()
        {
            var obj1 = provider.Resolve<IDependency>().First();
            var obj2 = provider.Resolve<IDependency>().First();
            Assert.AreNotSame(obj1, obj2);
        }

        [TestMethod]
        public void ImplementationsCountTest()
        {
            Assert.AreEqual(3, provider.Resolve<IDependency>().Count());
        }

        [TestMethod]
        public void AsSelfRegisterTest()
        {
            Assert.AreEqual(provider.Resolve<Class1>().First().GetType(), typeof(Class1));
        }

        [TestMethod]
        public void ConcreteGenericTest()
        {
            Assert.AreEqual(provider.Resolve<IService<MySqlRepository>>().First().GetType(), typeof(ServiceImpl<MySqlRepository>));
        }

        [TestMethod]
        public void OpenGenericTest()
        {
            Assert.AreEqual(provider.Resolve<IService<RepositoryImpl>>().First().GetType(), typeof(ServiceImpl<RepositoryImpl>));
        }

        [TestMethod]
        public void ConstructorWithDependency()
        {
            Assert.AreNotEqual(provider.Resolve<IDep>().FirstOrDefault(), null);
        }
    }
}
