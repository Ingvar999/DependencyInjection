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

        }
    }
}
