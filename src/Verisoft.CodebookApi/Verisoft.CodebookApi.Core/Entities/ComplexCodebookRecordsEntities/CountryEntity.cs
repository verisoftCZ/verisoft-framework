using Verisoft.CodebookApi.Core.Entities.BaseEntity;

namespace Verisoft.CodebookApi.Core.Entities.ComplexCodebookRecordsEntities;

public class CountryEntity : CodebookRecordEntity
{
    public string? IsoAlpha2 { get; set; }

    public string? IsoAlpha3 { get; set; }

    public string? IsoNumeric3 { get; set; }
}
