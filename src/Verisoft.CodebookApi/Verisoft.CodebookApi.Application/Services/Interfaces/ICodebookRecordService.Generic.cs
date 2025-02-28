using Verisoft.CodebookApi.Core.Entities.BaseEntity;

namespace Verisoft.CodebookApi.Application.Services.Interfaces;

public interface ICodebookRecordService<TCodebookRecordEntity> : ICodebookRecordService
    where TCodebookRecordEntity : ICodebookRecordEntity
{
}