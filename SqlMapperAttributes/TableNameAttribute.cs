using System;

namespace SqlMapperAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple=false)]
    public class TableNameAttribute : Attribute
    {
        private string tableName;

        public TableNameAttribute(string tableName)
        {
            this.tableName = tableName;
        }

        public override string ToString()
        {
            return tableName;
        }
    }
}
