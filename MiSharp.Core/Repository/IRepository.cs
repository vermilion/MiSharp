﻿using System.Linq;
using System.Threading.Tasks;

namespace MiSharp.Core.Repository
{
    public interface IRepository
    {
        IQueryable<T> GetAll<T>();
        Task Save<T>(T entity);
        Task Delete<T>(T entity);
    }
}