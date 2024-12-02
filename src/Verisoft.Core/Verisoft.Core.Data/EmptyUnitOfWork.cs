using System.Threading.Tasks;

namespace Verisoft.Core.Data
{
    public class EmptyUnitOfWork : IUnitOfWork
    {
        public Task CompleteAsync()
        {
            return Task.FromResult(0);
        }

        public void Dispose()
        {
        }
    }
}
