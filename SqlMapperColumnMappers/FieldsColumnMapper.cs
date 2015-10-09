using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using SqlMapperAttributes;

namespace SqlMapperColumnMapper
{
    public class FieldsColumnMapper : AbstractColumnMapper
    {
        public FieldsColumnMapper(Type objType, string connectionString, string tableName)
            : base(objType, connectionString, tableName) { }
        
        public override Dictionary<string, Type> MapMembersTypes()
        {
            Dictionary<string, Type> members = new Dictionary<string, Type>();

            IEnumerable<FieldInfo> allFields = typeT.GetFields().Where(field => ColumnNames.Contains(field.Name));
            foreach (FieldInfo field in allFields)
            {
                members.Add(field.Name, field.FieldType);
            }

            return members;
        }

        public override Dictionary<string, object> MapAllMembers(SqlDataReader dr)
        {
            Dictionary<string, object> members = new Dictionary<string, object>();
            IEnumerable<FieldInfo> allFields = typeT.GetFields()
                .Where(field => ColumnNames.Contains(field.Name));
            foreach (FieldInfo field in allFields)
            {
                members.Add(field.Name, dr.GetValue(getColumnIndexOfMemberWithName(field.Name)));
            }

            return members;
        }

        public override Dictionary<string, object> MapObjectValues(Object obj)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            IEnumerable<FieldInfo> allFields = typeT.GetFields()
                .Where(field => ColumnNames.Contains(field.Name));
            foreach (FieldInfo field in allFields)
            {
                values.Add(field.Name, field.GetValue(obj));
            }

            return values;
        }

        public override KeyValuePair<string, Type> MapMemberPrimaryKey(Type typeT)
        {
            foreach (FieldInfo field in typeT.GetFields())
            {
                Object[] attr = field.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                if (attr.Count() > 0)
                {
                    return new KeyValuePair<string, Type>(field.Name, field.FieldType);
                }
            }
            return new KeyValuePair<string,Type>();
        }

        public override object MapMembersIntoObject(Dictionary<string, object> members)
        {
            object newObject = Activator.CreateInstance(typeT);

            IEnumerable<FieldInfo> allFields = typeT.GetFields()
                .Where(field => ColumnNames.Contains(field.Name));
            foreach (FieldInfo field in allFields)
            {
                field.SetValue(newObject, members[field.Name]);
            }
            return newObject;
        }

        public override KeyValuePair<string, object> MapObjectPrimaryKey(Object obj)
        {
            KeyValuePair<string, Type> pK = MapMemberPrimaryKey(obj.GetType());
            return new KeyValuePair<string, object>(pK.Key, obj.GetType().GetField(pK.Key).GetValue(obj));
        }
    }
}
