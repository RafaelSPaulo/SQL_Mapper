using System;
using System.Data.SqlClient;

namespace SqlMapperConnectionPolicy
{
    public interface IConnectionPolicy : IDisposable
    {
        void OpenConnection();
        void CloseConnection();
        void BeginTransaction();
        void Commit();
        void RollBack();

        SqlConnection GetConnection();
        SqlTransaction GetTransaction();
        void DoBeforeCommandExecution();
        void DoAfterCommandExecution();
    }
}

