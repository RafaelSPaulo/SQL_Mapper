using System;

namespace SqlMapperConnectionPolicy
{
    public class SingleConnectionPolicy : AbstractConnectionPolicy
    {
        public SingleConnectionPolicy(String connectionString) : base(connectionString) { }

        public override void DoBeforeCommandExecution(){
            if (!connectionOpened)
            {
                OpenConnection();
            }
        }
        public override void DoAfterCommandExecution() {}
    }
}
