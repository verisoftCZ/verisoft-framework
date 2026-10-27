using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;

public class CodebookRegistry(string codebookRecordRoutePrefix, string codebookRecordTablePrefix) : ICodebookRegistry
{
    private readonly List<Type> codebookTypes = [];

    public string CodebookRecordRoutePrefix { get; init; } = codebookRecordRoutePrefix;

    public string CodebookRecordTablePrefix { get; init; } = codebookRecordTablePrefix;

    public void RegisterCodebook<T>()
        where T : CodebookRecordType, new()
    {
        var type = typeof(T);
        if (!codebookTypes.Contains(type))
        {
            codebookTypes.Add(type);
        }
    }

    public IReadOnlyList<Type> GetRegisteredCodebooks()
    {
        return codebookTypes.AsReadOnly();
    }
}
