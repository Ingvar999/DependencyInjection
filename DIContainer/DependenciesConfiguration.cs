using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer
{
    public class DependenciesConfiguration
    {
        private readonly Dictionary<Type, List<Type>> dependencies;
        public DependenciesConfiguration()
        {
            dependencies = new Dictionary<Type, List<Type>>();
        }
        public void Register<Dep, Impl>()
        {
            List<Type> list;
            if (!dependencies.TryGetValue(typeof(Dep), out list))
            {
                list = new List<Type>();
                dependencies.Add(typeof(Dep), list);
            }
            list.Add(typeof(Impl));
        }
        public IEnumerable<KeyValuePair<Type,List<Type>>> GetContent()
        {
            return dependencies.AsEnumerable();
        }
    }
}
