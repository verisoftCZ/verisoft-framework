using System.Threading.Tasks;
using System.Transactions;

namespace Verisoft.Core.Data
{
    public class BaseUnitOfWork : IUnitOfWork
    {
        private readonly TransactionScope transactionScope;

        public BaseUnitOfWork() => transactionScope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.RepeatableRead,
            },
            TransactionScopeAsyncFlowOption.Enabled);

        public Task CompleteAsync()
        {
            transactionScope.Complete();
            return Task.FromResult(0);
        }

        public void Dispose() => transactionScope.Dispose();
    }
}
