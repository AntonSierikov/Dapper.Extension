using System;
using System.Threading.Tasks;

namespace Dapper.Extension.Abstract
{
    public interface IQuery<T>
    {
        Task<T> GetAsync(Object obj);
    }
}
