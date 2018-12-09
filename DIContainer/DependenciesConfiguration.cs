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
        public void Register<Dep, Impl>()
        {
            List<Type> list;
            IEnumerable<Type> seq;
            if (!dependencies.TryGetValue(typeof(Dep), out seq))
            {
                list = new List<Type>();
                dependencies.Add(typeof(Dep), list);
            }
            else
            {
                list = seq as List<Type>;
            }
            list.Add(typeof(Impl));
        }
        public IEnumerable<KeyValuePair<Type, IEnumerable<Type>>> GetContent()
        {
            return dependencies.AsEnumerable();
        }
    }
}
