using System;
using System.Threading.Tasks;

namespace Verisoft.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task CompleteAsync();
    }
}
