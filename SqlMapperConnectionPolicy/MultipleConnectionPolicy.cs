using System;

namespace SqlMapperConnectionPolicy
{
    public class MultipleConnectionPolicy : AbstractConnectionPolicy
    {
        public MultipleConnectionPolicy(String connectionString) : base(connectionString) { }

        public override void DoBeforeCommandExecution()
        {
            if (connectionOpened)
            {
                CloseConnection();
            }
            OpenConnection();
        }
        public override void DoAfterCommandExecution()
        {
            CloseConnection();
        }
    }
}
