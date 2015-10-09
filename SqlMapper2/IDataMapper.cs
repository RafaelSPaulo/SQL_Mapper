using SqlMapperConnectionPolicy;

namespace SqlMapper2
{
    public interface IDataMapper<T>{
        ISqlEnumerable<T> GetAll();
        void Update(T val); 
        void Delete(T val);
        void Insert(T val);

        IConnectionPolicy GetConnectionPolicy();
    }
}
