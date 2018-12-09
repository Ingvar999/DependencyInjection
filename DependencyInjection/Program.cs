using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIContainer;

namespace DependencyInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new DependenciesConfiguration();
            config.Register<IDependency, Class1>();
            config.Register<IDependency, Class2>();
            config.Register<IDependency, Class11>();
            config.Register<Class1, Class1>();
            config.Register<Class1, Class11>();
            var provider = new DependencyProvider(config);
            var resolves = provider.Resolve<IDependency>();
            var t1 = typeof(List<int>);
            var t2 = typeof(List<>);
            var b1 = t1.ContainsGenericParameters;
            var b2 = t2.ContainsGenericParameters;
            var b = t1.GetGenericTypeDefinition().GetHashCode() == t2.GetHashCode();
            Console.ReadKey();
        }
    }
}
