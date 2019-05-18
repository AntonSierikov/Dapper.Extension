using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Extension.TypeExtensions
{
    public static class EnumerableExtension
    {

        //----------------------------------------------------------------//

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach(T item in enumerable)
            {
                action(item);
            }
        }

        //----------------------------------------------------------------//

    }
}
