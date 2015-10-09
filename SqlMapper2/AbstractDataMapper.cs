using System;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;

namespace SqlMapper2
{
    public abstract class AbstractDataMapper<T> : IDataMapper<T>
    {
        public IColumnMapper columnMapper;
        public IConnectionPolicy connectionPolicy;
        public string connectionString;

        public AbstractDataMapper(string connectionString, Type columnMapperType, Type connectionPolicyType) {
            this.connectionString = connectionString;
        }

        public abstract ISqlEnumerable<T> GetAll();
        public abstract void Update(T val); 
        public abstract void Delete(T val);
        public abstract void Insert(T val);

        public virtual IConnectionPolicy GetConnectionPolicy()
        { 
            return connectionPolicy;
        }
    }
}
