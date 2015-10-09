using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SqlMapperAttributes;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;


namespace SqlMapper3
{
    public class SqlDataMapper : IDataMapper
    {
        Dictionary<Type, IDataMapper> foreignMappers;
        public Type objType;
        public IColumnMapper columnMapper;
        public IConnectionPolicy connectionPolicy;
        public string connectionString;
        public string tableName;

        public SqlDataMapper(Type objType, string connectionString, Type columnMapperType, Type connectionPolicyType) {
            this.objType = objType;
            this.connectionString = connectionString;
            tableName = objType.GetCustomAttributes(typeof(TableNameAttribute), false).FirstOrDefault().ToString();
            connectionPolicy = (AbstractConnectionPolicy)connectionPolicyType.GetConstructor(new Type[] { typeof(String) })
                .Invoke(new Object[] { connectionString });

            columnMapper = (IColumnMapper)columnMapperType
                .GetConstructor(new Type[] { typeof(Type), typeof(String), typeof(String) })
                .Invoke(new Object[] { objType, connectionString, tableName });
            foreignMappers = BuildForeignKeyMappers();
        }

        private Dictionary<Type, IDataMapper> BuildForeignKeyMappers()
        {
            Dictionary<Type, IDataMapper> foreignMappers = new Dictionary<Type, IDataMapper>();
            List<Type> foreignTypes = columnMapper.MapMembersTypes().Select(pair => pair.Value)
                .Where(m => AbstractColumnMapper.TypeIsEDClass(m)).ToList();

            foreach (Type foreignType in foreignTypes)
            {
                foreignMappers.Add(foreignType,
                    new SqlDataMapper(foreignType, connectionString, columnMapper.GetType(), connectionPolicy.GetType()));
            }
            return foreignMappers;
        }

        public SqlEnumerable GetAll()
        {
            return new SqlEnumerable(connectionPolicy, columnMapper, foreignMappers, tableName);
        }

        public void Update(object val)
        {
            Dictionary<string, Type> eDMembers = columnMapper.MapEDMembersTypes();
            Dictionary<string, object> members = columnMapper.MapObjectValues(val);
            foreach (string name in eDMembers.Keys)
            {
                foreignMappers[eDMembers[name]].Update(members[name]);
            }

            connectionPolicy.DoBeforeCommandExecution();
            using (SqlCommand cmd = connectionPolicy.GetConnection().CreateCommand())
            {
                cmd.Transaction = connectionPolicy.GetTransaction();
                cmd.CommandText = SqlDataMapper.GetUpdateStringFor(columnMapper, tableName, val);
                cmd.ExecuteScalar();
            }
            connectionPolicy.DoAfterCommandExecution();
        }

        public void Delete(object val)
        {
            connectionPolicy.DoBeforeCommandExecution();
            using (SqlCommand cmd = connectionPolicy.GetConnection().CreateCommand())
            {
                cmd.Transaction = connectionPolicy.GetTransaction();
                cmd.CommandText = SqlDataMapper.GetDeleteStringFor(columnMapper, tableName, val);
                cmd.ExecuteScalar();
            }
            connectionPolicy.DoAfterCommandExecution();
        }

        public void Insert(object val)
        {
            connectionPolicy.DoBeforeCommandExecution();
            using (SqlCommand cmd = connectionPolicy.GetConnection().CreateCommand())
            {
                cmd.Transaction = connectionPolicy.GetTransaction();
                cmd.CommandText = GetInsertStringFor(columnMapper, tableName, val);
                cmd.ExecuteScalar();
            }
            connectionPolicy.DoAfterCommandExecution();
        }
        
        public object GetById(object pkValue)
        {
            KeyValuePair<string, Type> pair = columnMapper.MapMemberPrimaryKey(objType);
            SqlEnumerable all = GetAll().Where(GetWherePkStringFor(pair.Key, pkValue));
            if (all.Count() > 0)
                return all.First();
            else return null;
        }

        public static string GetUpdateStringFor(IColumnMapper columnMapper, string tableName, object val)
        {
            StringBuilder builder = new StringBuilder("UPDATE " + tableName + " SET ");

            Dictionary<string, object> values = columnMapper.MapObjectValues(val);
            KeyValuePair<string, object> primaryKey = columnMapper.MapObjectPrimaryKey(val);

            foreach (string key in values.Keys.Where(k => k != primaryKey.Key))
            {
                if (AbstractColumnMapper.TypeIsEDClass(values[key].GetType()))
                {
                    KeyValuePair<string, object> foreignKeyPair = columnMapper.MapObjectPrimaryKey(values[key]);
                    builder.AppendFormat("{0} = '{1}', ", foreignKeyPair.Key, foreignKeyPair.Value);
                }
                else { 
                    builder.AppendFormat("{0} = '{1}', ", key, values[key]);
                }
            }
            builder.Remove(builder.Length - 2, 2);

            builder.Append(" WHERE ");
            KeyValuePair<string, object> pair = columnMapper.MapObjectPrimaryKey(val);
            builder.Append(SqlDataMapper.GetWherePkStringFor(pair.Key, pair.Value));
            return builder.ToString();
        }

        public static string GetDeleteStringFor(IColumnMapper columnMapper, string tableName, object val)
        {
            StringBuilder builder = new StringBuilder("DELETE FROM " + tableName);
            builder.Append(" WHERE ");
            KeyValuePair<string, object> pair = columnMapper.MapObjectPrimaryKey(val);
            builder.Append(SqlDataMapper.GetWherePkStringFor(pair.Key, pair.Value));
            return builder.ToString();
        }

        public static string GetInsertStringFor(IColumnMapper columnMapper, string tableName, object val)
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

        public static string GetWherePkStringFor(string name, object value)
        {
            return string.Format("{0} = '{1}'", name, value);
        }
        
        public IConnectionPolicy GetConnectionPolicy()
        {
            return connectionPolicy;
        }
    }
    public class SqlDataMapper<T> : SqlDataMapper
    {
        public SqlDataMapper(string connectionString, Type columnMapperType, Type connectionPolicyType)
            : base(typeof(T), connectionString, columnMapperType, connectionPolicyType){}
    }
}