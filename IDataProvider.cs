using System.Collections.Generic;

namespace Architecture
{
    public interface IDataProvider<T>
    {
        IEnumerable<T> GetData();
    }
}