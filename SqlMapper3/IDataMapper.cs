using SqlMapperConnectionPolicy;

namespace SqlMapper3
{
    public interface IDataMapper {
        SqlEnumerable GetAll();
        void Update(object val);
        void Delete(object val);
        void Insert(object val);
        object GetById(object pkValue);
        IConnectionPolicy GetConnectionPolicy();
    }

    public interface IDataMapper<T> : IDataMapper{
        new SqlEnumerable<T> GetAll();
        void Update(T val); 
        void Delete(T val);
        void Insert(T val);
    }
}
