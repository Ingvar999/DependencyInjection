using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer
{
    public class DependenciesConfiguration
    {
        private readonly Dictionary<Type, IEnumerable<Type>> dependencies;
        public DependenciesConfiguration()
        {
            dependencies = new Dictionary<Type, IEnumerable<Type>>();
        }
        public void Register<Dep, Impl>() where Dep : class
        {
            Register(typeof(Dep), typeof(Impl));
        }

        public void Register(Type dep, Type impl)
        {
            List<Type> list;
            IEnumerable<Type> seq;
            if (!dependencies.TryGetValue(dep, out seq))
            {
                list = new List<Type>();
                dependencies.Add(dep, list);
            }
            else
            {
                list = seq as List<Type>;
            }
            list.Add(impl);
        }
        public IEnumerable<KeyValuePair<Type, IEnumerable<Type>>> GetContent()
        {
            return dependencies.AsEnumerable();
        }
    }
}
