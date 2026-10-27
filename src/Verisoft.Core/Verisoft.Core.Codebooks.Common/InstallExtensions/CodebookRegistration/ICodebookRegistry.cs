namespace Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;

public interface ICodebookRegistry
{
    string CodebookRecordRoutePrefix { get; }

    string CodebookRecordTablePrefix { get; }

    IReadOnlyList<Type> GetRegisteredCodebooks();
}
