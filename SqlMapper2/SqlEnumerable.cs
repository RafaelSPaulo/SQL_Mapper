using System.Collections.Generic;
using System.Data.SqlClient;
using SqlMapperColumnMapper;
using SqlMapperConnectionPolicy;

namespace SqlMapper2
{
    public class SqlEnumerable<T> : ISqlEnumerable<T>
    {
        private IConnectionPolicy connectionPolicy;
        private IColumnMapper columnMapper;
        private string tableName;
        private string whereClause;
        private List<T> results;
    
        public SqlEnumerable(IConnectionPolicy connectionPolicy, IColumnMapper columnMapper, string tableName, string whereClause = "") {
            this.connectionPolicy = connectionPolicy;
            this.columnMapper = columnMapper;
            this.tableName = tableName;
            this.whereClause = whereClause;
            results = null;
        }

        public ISqlEnumerable<T> Where(string clause) {
            string enumerableWhereClause = 
                string.IsNullOrEmpty(whereClause) ?
                    clause :
                    whereClause + " AND " + clause;
            return new SqlEnumerable<T>(connectionPolicy, columnMapper, tableName, enumerableWhereClause);
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (results == null)
            {
                results = new List<T>();
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
                            /*...*/
                            T newObject = (T)columnMapper.MapMembersIntoObject(members);
                            results.Add(newObject);
                        }
                    }
                }
                connectionPolicy.DoAfterCommandExecution();
            }
            foreach (T result in results) { 
                yield return result;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        ISqlEnumerable<T> ISqlEnumerable<T>.Where(string clause)
        {
            return this.Where(clause);
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
