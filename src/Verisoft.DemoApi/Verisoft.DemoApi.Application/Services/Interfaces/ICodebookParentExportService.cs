using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.CodebookParent;

namespace Verisoft.DemoApi.Application.Services.Interfaces;

public interface ICodebookParentExportService : IBaseExportService<CodebookParentResponse, CodebookParentFilter, int>;
