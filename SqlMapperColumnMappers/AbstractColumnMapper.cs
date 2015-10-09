using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using SqlMapperAttributes;

namespace SqlMapperColumnMapper
{
    public abstract class AbstractColumnMapper : IColumnMapper
    {
        public Type typeT;
        public string connectionString;
        public string tableName;
        private List<String> columnNames;
        public List<String> ColumnNames
        {
            get
            {
                if (columnNames == null)
                {
                    using (SqlConnection conSql = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = conSql.CreateCommand())
                        {
                            cmd.CommandText =
                                string.Format(
                                "SELECT TOP 0 * FROM {0}", tableName);
                            conSql.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            List<String> fieldNames = new List<String>();
                            for (int i = 0; i < dr.FieldCount; ++i)
                                fieldNames.Add(dr.GetName(i));
                            columnNames = fieldNames;
                        }
                    }
                }
                return columnNames;
            }
        }
        
        public AbstractColumnMapper(Type objType, string connectionString, string tableName)
        {
            this.typeT = objType;
            this.connectionString = connectionString;
            this.tableName = tableName;
        }

        public int getColumnIndexOfMemberWithName(string name)
        {
            try
            {
                return ColumnNames.IndexOf(name);
            }
            catch
            {
                return -1;
            }
        }

        public Dictionary<string, Type> MapEDMembersTypes() {
            Dictionary<string, Type> allEDMembers = new Dictionary<string, Type>();
            Dictionary<string, Type> allMembers = MapMembersTypes();
            foreach (string name in allMembers.Keys) {
                if (TypeIsEDClass(allMembers[name])) {
                    allEDMembers.Add(name, allMembers[name]);
                }
            }
            return allEDMembers;
        }
        
        public static bool TypeIsEDClass(Type type)
        {
            return type.GetCustomAttributes(typeof(TableNameAttribute), false).Length > 0;
        }

        public abstract Dictionary<string, Type> MapMembersTypes();
        public abstract Dictionary<string, object> MapAllMembers(SqlDataReader dr);
        public abstract Dictionary<string, object> MapObjectValues(Object obj);
        public abstract KeyValuePair<string, object> MapObjectPrimaryKey(Object obj);
        public abstract KeyValuePair<string, Type> MapMemberPrimaryKey(Type typeT);
        public abstract object MapMembersIntoObject(Dictionary<string, object> members);
    }
}
