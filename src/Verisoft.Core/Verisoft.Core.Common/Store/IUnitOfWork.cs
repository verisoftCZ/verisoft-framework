namespace Verisoft.Core.Common.Store;

public interface IUnitOfWork
{
    void Commit();

    void Rollback();
}
