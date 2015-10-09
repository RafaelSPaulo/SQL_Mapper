using System.Collections.Generic;

namespace SqlMapper3
{
    public interface ISqlEnumerable<T> : IEnumerable<T>
    {
        ISqlEnumerable<T> Where(string clause);
    }
}
