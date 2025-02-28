using System.Linq.Expressions;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.Core.Common.Entities;

namespace Verisoft.CodebookApi.Core.Sorts
{
    public class CodebookSort(string propertyName, bool isAscending, bool defaultAsc) : BaseSort<CodebookEntity>(propertyName, isAscending, defaultAsc)
    {
        public override Expression<Func<CodebookEntity, object>> DefaultSort
        {
            get
            {
                return x => x.Name;
            }
        }

        protected override Expression<Func<CodebookEntity, object>> GetSortingExpression(string propertyName)
        {
            return propertyName switch
            {
                "name" => x => x.Name,
                "id" => x => x.Id,
                "hashardcodedenum" => x => x.HasHardcodedEnum,
                "isbasictype" => x => x.IsBasicType,
                "createdby" => x => x.CreatedBy,
                "createdat" => x => x.CreatedAt,
                "updatedby" => x => x.UpdatedBy,
                "updatedat" => x => x.UpdatedAt ?? default,
                "isexternal" => x => x.IsExternal,
                _ => DefaultSort,
            };
        }
    }
}
