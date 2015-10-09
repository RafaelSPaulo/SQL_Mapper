using System;

namespace SqlMapperConnectionPolicy
{
    public class ExplicitConnectionPolicy : AbstractConnectionPolicy
    {
        public ExplicitConnectionPolicy(String connectionString) : base(connectionString) { }
        public override void DoBeforeCommandExecution() {}
        public override void DoAfterCommandExecution() {}
    }
}
