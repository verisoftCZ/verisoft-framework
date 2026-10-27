using Verisoft.Core.AspNet.Services;
using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Common.Sorts;
using Verisoft.DemoApi.Common.Specifications;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.CodebookParent;

namespace Verisoft.DemoApi.Application.Services;

public class CodebookParentExportService(
    ICodebookParentRepository repository,
    ITypeMapper typeMapper,
    IExportService<CodebookParentResponse> exportService)
    : BaseExportService<CodebookParentResponse, CodebookParentFilter, CodebookParentSpecification, CodebookParentSort, CodebookParentEntity, int>(typeMapper, repository, exportService), ICodebookParentExportService;
