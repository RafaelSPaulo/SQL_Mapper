using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SqlMapperAttributes;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;


namespace SqlMapper1
{
    public class SqlDataMapper<T> : AbstractDataMapper<T>
    {
        private string tableName;
        private Type typeT;

        public SqlDataMapper(string connectionString, Type columnMapperType, Type connectionPolicyType)
            : base(connectionString, columnMapperType, connectionPolicyType)
        {
            typeT = this.GetType().GenericTypeArguments.FirstOrDefault();
            tableName = typeT.GetCustomAttributes(typeof(TableNameAttribute), false).FirstOrDefault().ToString();
            columnMapper = 
                (IColumnMapper)columnMapperType
                .GetConstructor(new Type[] { typeof(Type), typeof(String), typeof(String) })
                .Invoke(new Object[] { typeT, connectionString, tableName });
            connectionPolicy = (AbstractConnectionPolicy)connectionPolicyType.GetConstructor(new Type[] { typeof(String) })
                .Invoke(new Object[] { connectionString });
        }

        public override IEnumerable<T> GetAll()
        {
            List<T> results = new List<T>();
            connectionPolicy.DoBeforeCommandExecution();
            using (SqlCommand cmd = connectionPolicy.GetConnection().CreateCommand())
            {
                cmd.Transaction = connectionPolicy.GetTransaction();
                cmd.CommandText = "SELECT * FROM " + tableName;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Dictionary<string, object> members = columnMapper.MapAllMembers(dr);
                        /*...*/
                        T newObject = (T)columnMapper.MapMembersIntoObject(members);
                        results.Add(newObject);
                    }
                }
            }
            connectionPolicy.DoAfterCommandExecution();
            foreach (T result in results)
            {
                yield return result;
            }
        }

        public override void Update(T val)
        {
            connectionPolicy.DoBeforeCommandExecution();
            using (SqlCommand cmd = connectionPolicy.GetConnection().CreateCommand())
            {
                cmd.Transaction = connectionPolicy.GetTransaction();
                cmd.CommandText = GetUpdateStringFor(val);
                cmd.ExecuteScalar();
            }
            connectionPolicy.DoAfterCommandExecution();
        }

        public override void Delete(T val)
        {
            connectionPolicy.DoBeforeCommandExecution();
            using (SqlCommand cmd = connectionPolicy.GetConnection().CreateCommand())
            {
                cmd.Transaction = connectionPolicy.GetTransaction();
                cmd.CommandText = GetDeleteStringFor(val);
                cmd.ExecuteScalar();
            }
            connectionPolicy.DoAfterCommandExecution();
        }

        public override void Insert(T val)
        {
            connectionPolicy.DoBeforeCommandExecution();
            using (SqlCommand cmd = connectionPolicy.GetConnection().CreateCommand())
            {
                cmd.Transaction = connectionPolicy.GetTransaction();
                cmd.CommandText = GetInsertStringFor(val);
                cmd.ExecuteScalar();
            }
            connectionPolicy.DoAfterCommandExecution();
        }

        public string GetUpdateStringFor(object val)
        {
            StringBuilder builder = new StringBuilder("UPDATE " + tableName + " SET ");
            
            Dictionary<string, object> values = columnMapper.MapObjectValues(val);
            KeyValuePair<string, object> primaryKey = columnMapper.MapObjectPrimaryKey(val);

            foreach (string key in values.Keys.Where(k => k != primaryKey.Key))
            {
                builder.AppendFormat("{0} = '{1}', ", key, values[key]);
            }
            builder.Remove(builder.Length - 2, 2);

            builder.Append(" WHERE ");
            builder.Append(GetWherePkStringFor(val));
            return builder.ToString();
        }

        public string GetDeleteStringFor(Object val)
        {
            StringBuilder builder = new StringBuilder("DELETE FROM " + tableName);
            builder.Append(" WHERE ");
            builder.Append(GetWherePkStringFor(val));
            return builder.ToString();
        }

        public string GetInsertStringFor(Object val)
        {
            StringBuilder names = new StringBuilder();
            StringBuilder values = new StringBuilder();

            Dictionary<string, object> objectValues = columnMapper.MapObjectValues(val);

            foreach (string key in objectValues.Keys)
            {
                names.Append(key + ", ");
                values.Append("'" + objectValues[key]);
            }
            names.Remove(names.Length - 2, 2);
            values.Remove(values.Length - 2, 2);

            return String.Format("INSERT INTO {0}({1}) VALUES({2})", tableName, names, values);
        }

        public string GetWherePkStringFor(object val)
        {
            KeyValuePair<string, object> pair = columnMapper.MapObjectPrimaryKey(val);
            return string.Format("{0} = '{1}'", pair.Key, pair.Value);
        }
    }
}



