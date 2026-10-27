using System.ComponentModel.DataAnnotations;

namespace Verisoft.DemoApi.Common.Enums;

public enum CodebookParentType
{
    [Display(Name = "Irelevantní")]
    Irrelevant = 0,

    [Display(Name = "Nesmyslný")]
    Nonsensical = 1,
}