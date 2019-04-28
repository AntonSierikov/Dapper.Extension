using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extension.Abstract
{
    public interface ICommand<T, TKey> where T : class
    {
        Task<Int32> DeleteAsync(TKey key);
        Task<TKey> InsertAsync(T entity);
        Task<Int32> UpdateAsync(T entity);        
    }
}
