using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;

namespace SqlMapper3
{
    public class SqlEnumerable : IEnumerable<object>{
        private IConnectionPolicy connectionPolicy;
        private IColumnMapper columnMapper;
        private string tableName;
        private string whereClause;
        private List<object> results;
        private Dictionary<Type, IDataMapper> foreignMappers;

        public SqlEnumerable(IConnectionPolicy connectionPolicy, IColumnMapper columnMapper,
            Dictionary<Type, IDataMapper> foreignMappers, string tableName, string whereClause = "")
        {
            this.connectionPolicy = connectionPolicy;
            this.columnMapper = columnMapper;
            this.tableName = tableName;
            this.whereClause = whereClause;
            results = null;
            this.foreignMappers = foreignMappers;
        }

        public SqlEnumerable Where(string clause)
        {
            string enumerableWhereClause =
                string.IsNullOrEmpty(whereClause) ?
                    clause :
                    whereClause + " AND " + clause;
            return new SqlEnumerable(connectionPolicy, columnMapper, foreignMappers, tableName, enumerableWhereClause);
        }

        public IEnumerator<object> GetEnumerator()
        {
            if (results == null)
            {
                results = new List<object>();
                Dictionary<string, Type> allEDMembers = columnMapper.MapEDMembersTypes();

                connectionPolicy.DoBeforeCommandExecution();
                using (SqlCommand cmd = connectionPolicy.GetConnection().CreateCommand())
                {
                    cmd.Transaction = connectionPolicy.GetTransaction();
                    cmd.CommandText = "SELECT * FROM " + tableName;
                    if (!string.IsNullOrEmpty(whereClause))
                    {
                        cmd.CommandText = string.Format("{0} WHERE {1}", cmd.CommandText, whereClause);
                    }

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Dictionary<string, object> members = columnMapper.MapAllMembers(dr);
                            foreach (string name in allEDMembers.Keys)
                            {
                                IDataMapper dataMapper = foreignMappers[allEDMembers[name]];
                                object foreignObject = dataMapper.GetById(members[name]);
                                members[name] = foreignObject;
                            }

                            object newObject = columnMapper.MapMembersIntoObject(members);
                            results.Add(newObject);
                        }
                    }
                }
                connectionPolicy.DoAfterCommandExecution();
            }
            foreach (object obj in results)
                yield return obj;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    public class SqlEnumerable<T> : SqlEnumerable
    {
        public SqlEnumerable(IConnectionPolicy connectionPolicy, IColumnMapper columnMapper, 
            Dictionary<Type, IDataMapper> foreignMappers, string tableName, string whereClause = "")
        :base(connectionPolicy, columnMapper, foreignMappers, tableName, whereClause){}
    }
}
