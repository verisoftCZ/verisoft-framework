namespace Verisoft.Core.Data.EntityFramework.Walkers;

public interface IWalkerFactory
{
    CascadeWalker CreateCascadeWalker(EntityFrameworkDbContextBase context);
}
