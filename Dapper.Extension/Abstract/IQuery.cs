using System.Threading.Tasks;

namespace Dapper.Extension.Abstract
{
    public interface IQuery<T, TKey>
    {
        Task<T> GetAsync(TKey key);
    }
}
