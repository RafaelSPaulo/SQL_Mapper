using System.Collections.Generic;

namespace SqlMapper2
{
    public interface ISqlEnumerable<T> : IEnumerable<T>
    {
        ISqlEnumerable<T> Where(string clause);
    }
}
