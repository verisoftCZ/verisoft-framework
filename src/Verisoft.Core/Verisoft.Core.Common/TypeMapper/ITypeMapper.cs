using System.Collections.Generic;

namespace Verisoft.Core.Common.TypeMapper
{
    public interface ITypeMapper
    {
        TTarget Map<TSource, TTarget>(TSource source);

        TTarget Map<TSource, TTarget>(TSource source, TTarget target);

        TTarget Map<TTarget>(object source);

        IEnumerable<TTarget> Map<TTarget>(IEnumerable<object> source);
    }
}
