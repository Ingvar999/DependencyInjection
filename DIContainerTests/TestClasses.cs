﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainerTests
{

    public interface IDependency { }
    public class Class1 : IDependency { }
    public class Class2 : IDependency { }
    public class Class11 : Class1 { }
    public interface IRepository { }
    public class RepositoryImpl : IRepository { }
    public class MySqlRepository : IRepository { }
    public interface IService<TRepository> where TRepository : IRepository { }
    public class ServiceImpl<TRepository> : IService<TRepository>
    where TRepository : IRepository
    {
        public ServiceImpl()
        {
        }
    }
    public interface IDep { }
    public class DependencyClass : IDep {
        public DependencyClass(IDependency dependency) { }
    }
}
