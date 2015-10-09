using System.Collections.Generic;
using SqlMapperConnectionPolicy;

namespace SqlMapper1
{
    public interface IDataMapper<T>{
        IEnumerable<T> GetAll();
        void Update(T val); 
        void Delete(T val);
        void Insert(T val);

        IConnectionPolicy GetConnectionPolicy();
    }
}
