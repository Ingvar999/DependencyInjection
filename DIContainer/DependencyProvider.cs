using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer
{
    public class DependencyProvider
    {
        private readonly Dictionary<Type, List<Type>> dependencies;
        private readonly ConcurrentDictionary<Type, object> singletonObjects;
        private readonly bool isSingleton;

        public DependencyProvider(DependenciesConfiguration config, bool singletonPolicy = false)
        {
            isSingleton = singletonPolicy;
            singletonObjects = new ConcurrentDictionary<Type, object>();
            dependencies = new Dictionary<Type, List<Type>>();
            foreach(var dependency in config.GetContent())
            {
                Type dep = dependency.Key;
                var list = new List<Type>();
                foreach(var impl in dependency.Value)
                {
                    if ((dep.IsAssignableFrom(impl) && !impl.IsAbstract && !impl.IsInterface) || (dep.IsGenericTypeDefinition))
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

        private IEnumerable<object> Resolve(Type T)
        {
            if (T.IsGenericType)
            {
                if (T.GetGenericArguments().Length > 1)
                {
                    throw new Exception("Too many generic parameters");
                }
                else
                {
                    return ResolveGenericType(T);
                }
            }
            else
            {
                return ResolveSimpleType(T);
            }
        }

        public IEnumerable<object> ResolveSimpleType(Type T)
        {
            var objects = new List<object>();
            if (dependencies.TryGetValue(T, out List<Type> types))
            {
                foreach (Type type in types)
                {
                    objects.Add(GetInstance(type));
                }
            }

            return objects;
        }

        private IEnumerable<object> ResolveGenericType(Type T)
        {
            var objects = new List<object>(ResolveSimpleType(T));
            if (objects.Count == 0)
            {
                if (dependencies.TryGetValue(T.GetGenericTypeDefinition(), out List<Type> types))
                {
                    foreach (Type type in types)
                    {
                        objects.Add(GetInstance(type.MakeGenericType(T.GetGenericArguments())));
                    }
                }
            }
            return objects;
        }

        private object GetInstance(Type T)
        {
            if (isSingleton)
            {
                return singletonObjects.GetOrAdd(T, CreateInstanceByConstructor);
            }
            else
            {
                return CreateInstanceByConstructor(T);
            }
        }

        private object CreateInstanceByConstructor(Type T)
        {
            var constructor = T.GetConstructors()[0];
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
                    throw new Exception("Require not registered type");
                }
            }
            return constructor.Invoke(args);
        }
    }
}
