using System;
using System.Collections.Generic;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;

namespace SqlMapper1
{
    public abstract class AbstractDataMapper<T> : IDataMapper<T>
    {
        public IColumnMapper columnMapper;
        public IConnectionPolicy connectionPolicy;
        public string connectionString;

        public AbstractDataMapper(string connectionString, Type columnMapperType, Type connectionPolicyType) {
            this.connectionString = connectionString;
        }

        public abstract IEnumerable<T> GetAll();
        public abstract void Update(T val); 
        public abstract void Delete(T val);
        public abstract void Insert(T val);

        public virtual IConnectionPolicy GetConnectionPolicy()
        { 
            return connectionPolicy;
        }
    }
}
