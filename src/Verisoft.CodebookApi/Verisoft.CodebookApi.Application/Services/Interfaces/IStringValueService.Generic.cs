using Verisoft.CodebookApi.Core.Entities.BaseEntity;

namespace Verisoft.CodebookApi.Application.Services.Interfaces;

public interface IStringValueService<TCodebookRecordEntity> : IStringValueService
    where TCodebookRecordEntity : ICodebookRecordEntity
{
}