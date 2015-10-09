using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace SqlMapperColumnMapper
{
    public interface IColumnMapper
    {
        Dictionary<string, Type> MapMembersTypes();
        Dictionary<string, object> MapAllMembers(SqlDataReader dr);
        Dictionary<string, Type> MapEDMembersTypes();
        Dictionary<string, object> MapObjectValues(Object obj);
        KeyValuePair<string, object> MapObjectPrimaryKey(Object obj);
        KeyValuePair<string, Type> MapMemberPrimaryKey(Type typeT);
        object MapMembersIntoObject(Dictionary<string, object> members);
    }
}