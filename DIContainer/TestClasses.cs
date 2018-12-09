using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer
{

    public interface IDependency
    {

    } 
    public class Class1 : IDependency
    {

    }

    public class Class2 : IDependency
    {

    }
    public class Class11 : Class1
    {

    }
    public interface IRepository
    {

    }
    public class RepositoryImpl : IRepository
    {

    }
    public interface IService<TRepository> where TRepository : IRepository
    {

    }

    class ServiceImpl<TRepository> : IService<TRepository>
    where TRepository : IRepository
    {
        public ServiceImpl(TRepository repository)
        {
        }
    }
}
