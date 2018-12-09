using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer
{
    public class DependencyProvider
    {
        private readonly Dictionary<Type, List<Type>> dependencies;

        public DependencyProvider(DependenciesConfiguration config)
        {
            dependencies = new Dictionary<Type, List<Type>>();
            foreach(var dependency in config.GetContent())
            {
                Type dep = dependency.Key;
                var list = new List<Type>();
                foreach(var impl in dependency.Value)
                {
                    if (dep.IsAssignableFrom(impl) && !impl.IsAbstract && !impl.IsInterface)
                    {
                        list.Add(impl);
                    }
                }
                if (list.Count != 0)
                {
                    dependencies.Add(dep, list);
                }
            }
        }

        public IEnumerable<TDep> Resolve<TDep>() where TDep : class
        {
            var result = new List<TDep>();
            foreach (var obj in Resolve(typeof(TDep)))
            {
                result.Add(obj as TDep);
            }
            return result;
        }

        private IEnumerable<object> Resolve(Type dep)
        {
            var objects = new List<object>();
            List<Type> types;
            if (dependencies.TryGetValue(dep, out types))
            {
                foreach (Type type in types)
                {
                    var constructor = type.GetConstructors()[0];
                    if (constructor.GetParameters().Length == 0)
                    {
                       objects.Add(Activator.CreateInstance(type));
                    }
                    else
                    {
                        object[] args = new object[constructor.GetParameters().Length];
                        int i = 0;
                        foreach (var arg in constructor.GetParameters())
                        {
                            if (dependencies.ContainsKey(arg.ParameterType))
                            {
                                args[i++] = Resolve(arg.ParameterType).FirstOrDefault();
                            }
                            else
                            {
                                throw new Exception("Cannot find type of dependency");
                            }
                        }
                        objects.Add(constructor.Invoke(args));
                    }
                }
            }

            return objects;
        }
    }
}
