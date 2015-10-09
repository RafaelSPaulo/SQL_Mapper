using System;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;

namespace SqlMapper3
{
    public abstract class AbstractDataMapper<T> : IDataMapper<T>
    {
        public IColumnMapper columnMapper;
        public IConnectionPolicy connectionPolicy;
        public string connectionString;

        public AbstractDataMapper(string connectionString, Type columnMapperType, Type connectionPolicyType) {
            this.connectionString = connectionString;
        }

        public abstract SqlEnumerable<T> GetAll();
        public abstract void Update(T val); 
        public abstract void Delete(T val);
        public abstract void Insert(T val);
        public abstract T GetById(object pkValue);
        public void Update(object val) {
            throw new NotImplementedException();    
        }
        public void Delete(object val)
        {
            throw new NotImplementedException();
        }
        public void Insert(object val)
        {
            throw new NotImplementedException();
        }

        SqlEnumerable<T> IDataMapper<T>.GetAll() {
            return this.GetAll();
        }

        SqlEnumerable IDataMapper.GetAll(){
            throw new NotImplementedException();
        }

        object IDataMapper.GetById(object pkValue)
        {
            return this.GetById(pkValue);
        }

        public virtual IConnectionPolicy GetConnectionPolicy()
        {
            return connectionPolicy;
        }
    }
}
