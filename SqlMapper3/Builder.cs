using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SqlMapperAttributes;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;

namespace SqlMapper3
{
    public class Builder
    {
        private Type genericMapperType;
        private List<Object> dataMapperCtorParams;
        private Type columnMapperType;
        private Type connectionPolicyType;
        
        public Builder(Type genericMapperType, Object[] dataMapperCtorParams) : 
            this(genericMapperType, dataMapperCtorParams, typeof(PropertyColumnMapper), typeof(MultipleConnectionPolicy)){}
                
        public Builder(Type genericMapperType, Object[] dataMapperCtorParams, Type columnMapperType, Type connectionPolicyType)
        {
            this.genericMapperType = genericMapperType;
            this.dataMapperCtorParams = dataMapperCtorParams.ToList();
            this.columnMapperType = columnMapperType;
            this.connectionPolicyType = connectionPolicyType;
        }

        public IDataMapper Build<T>() {
            Object[] tableNames = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), false);
            if (tableNames.Count() < 1 || !typeof(T).IsClass)
            {
                return null;
            }
            
            Type dataMapperType     = genericMapperType.MakeGenericType(new Type[] { typeof(T) });
            dataMapperCtorParams.Add(columnMapperType);
            dataMapperCtorParams.Add(connectionPolicyType);
            
            Type[] ctorParamsTypes  = dataMapperCtorParams.Select(p => p.GetType()).ToArray();
            var dataMapperCtor = dataMapperType.GetConstructor(ctorParamsTypes);
            
            return dataMapperCtor != null ?
                (IDataMapper)dataMapperCtor.Invoke(dataMapperCtorParams.ToArray()) :
                null;
        }
    }
}
