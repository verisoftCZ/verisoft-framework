using System.Collections.Generic;

namespace Verisoft.Core.Contracts
{
    public class PagedData<TEntity>
    {
        public int Total { get; set; }

        public IEnumerable<TEntity> Data { get; set; }
    }
}
