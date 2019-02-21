using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Serialization.Json
{

    /// <summary>
    /// Json.net support for using an array pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonNetArrayPool<T> : IArrayPool<T>
    {
        public JsonNetArrayPool(ArrayPool<T> PoolToUse)
        {
            Pool = PoolToUse;
        }

        private ArrayPool<T> Pool { get; }

        public T[] Rent(int minimumLength)
        {
            return Pool.Rent(minimumLength);
        }

        public void Return(T[] array)
        {
            Pool.Return(array);
        }
    }
}
