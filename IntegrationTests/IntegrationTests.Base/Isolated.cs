using System.Reflection;
using System.Transactions;
using Xunit.Sdk;

namespace IntegrationTests.Base
{
    public class Isolated : BeforeAfterTestAttribute
    {
        private TransactionScope _transactionScope;

        public override void Before(MethodInfo methodUnderTest)
        {
            _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        public override void After(MethodInfo methodUnderTest)
        {
            _transactionScope.Dispose();
        }
    }
}
