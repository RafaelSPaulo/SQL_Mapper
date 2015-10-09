using System;
using System.Data.SqlClient;

namespace SqlMapperConnectionPolicy
{
    public abstract class AbstractConnectionPolicy : IConnectionPolicy
    {
        private System.Data.IsolationLevel ISOLATIONLEVEL = System.Data.IsolationLevel.Serializable;

        private string connectionString;
        public SqlConnection connection;
        public SqlTransaction transaction;
        public bool connectionOpened;
        public bool transactionOpened;

        public AbstractConnectionPolicy(String connectionString)
        {
            this.connectionString = connectionString;
            connectionOpened = false;
            transactionOpened = false;
        }

        public SqlConnection GetConnection() { return connection; }
        public SqlTransaction GetTransaction() { return transaction; }

        public void OpenConnection()
        {
            if (!connectionOpened)
            {
                try
                {
                    this.connection = new SqlConnection(connectionString);
                    connection.Open();
                    connectionOpened = true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void CloseConnection()
        {
            if (connectionOpened)
            {
                if (transactionOpened)
                {
                    throw new InvalidOperationException("A Transaction waits for a Commit or RollBack operation.");
                }
                else
                {
                    try
                    {
                        if (transaction != null)
                        {
                            transaction.Dispose();
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    try
                    {
                        if (connection != null)
                        {
                            connection.Close();
                            connectionOpened = false;
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            connection.Dispose();
        }

        public void BeginTransaction()
        {
            if (!connectionOpened)
            {
                throw new InvalidOperationException("No Connection opened.");
            }
            else
            {
                if (transactionOpened)
                {
                    throw new InvalidOperationException("Transaction already opened.");
                }
                else
                {
                    try
                    {
                        transaction = connection.BeginTransaction(ISOLATIONLEVEL);
                        transactionOpened = true;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public void Commit()
        {
            if (!connectionOpened)
            {
                throw new InvalidOperationException("No Connection opened.");
            }
            else
            {
                if (transactionOpened)
                {
                    try
                    {
                        transaction.Commit();
                        transactionOpened = false;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public void RollBack()
        {
            if (!connectionOpened)
            {
                throw new InvalidOperationException("No Connection opened.");
            }
            else
            {
                if (transactionOpened)
                {
                    try
                    {
                        transaction.Rollback();
                        transactionOpened = false;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public abstract void DoBeforeCommandExecution();
        public abstract void DoAfterCommandExecution();

        public void Dispose()
        {
            RollBack();
            CloseConnection();
        }
    }
}
