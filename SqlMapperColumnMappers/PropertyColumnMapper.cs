using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using SqlMapperAttributes;

namespace SqlMapperColumnMapper
{
    public class PropertyColumnMapper : AbstractColumnMapper
    {
        public PropertyColumnMapper(Type objType, string connectionString, string tableName)
            : base(objType, connectionString, tableName) { }

        public override Dictionary<string, Type> MapMembersTypes() 
        { 
            Dictionary<string, Type> members = new Dictionary<string, Type>();

            IEnumerable<PropertyInfo> allProps = typeT.GetProperties().Where(prop => ColumnNames.Contains(prop.Name));
            foreach (PropertyInfo prop in allProps) 
            { 
                members.Add(prop.Name, prop.PropertyType);
            }

            return members;
        }

        public override Dictionary<string, object> MapAllMembers(SqlDataReader dr)
        {
            Dictionary<string, object> members = new Dictionary<string, object>();
            IEnumerable<PropertyInfo> allProps = typeT.GetProperties()
                .Where(prop => ColumnNames.Contains(prop.Name));
            foreach (PropertyInfo prop in allProps) { 
                members.Add(prop.Name, dr.GetValue(getColumnIndexOfMemberWithName(prop.Name)));
            }

            return members;
        }

        public override Dictionary<string, object> MapObjectValues(Object obj)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            IEnumerable<PropertyInfo> allProps = typeT.GetProperties().Where(prop => ColumnNames.Contains(prop.Name));
            foreach (PropertyInfo prop in allProps)
            {
                values.Add(prop.Name, prop.GetGetMethod().Invoke(obj, null));
            }

            return values;
        }

        public override KeyValuePair<string, Type> MapMemberPrimaryKey(Type typeT)
        {
            foreach (PropertyInfo prop in typeT.GetProperties())
            {
                Object[] attr = prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                if (attr.Count() > 0)
                {
                    return new KeyValuePair<string, Type>(prop.Name, prop.PropertyType);
                }
            }
            return new KeyValuePair<string, Type>();
        }

        public override object MapMembersIntoObject(Dictionary<string, object> members)
        {
            object newObject = Activator.CreateInstance(typeT);

            IEnumerable<PropertyInfo> allProps = typeT.GetProperties()
                .Where(prop => ColumnNames.Contains(prop.Name));
            foreach (PropertyInfo prop in allProps)
            {
                prop.GetSetMethod().Invoke(
                        newObject, new object[] { members[prop.Name] });
            }
            return newObject;
        }
       
        public override KeyValuePair<string, object> MapObjectPrimaryKey(Object obj)
        {
            KeyValuePair<string, Type> pK = MapMemberPrimaryKey(obj.GetType());
            return new KeyValuePair<string, object>(pK.Key, obj.GetType().GetProperty(pK.Key).GetGetMethod().Invoke(obj, null));
        }
    }
}
