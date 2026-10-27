using System.Reflection;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;

public static class CodebookDiscovery
{
    public static IReadOnlyCollection<Type> DiscoverFromAssembly(Assembly assembly)
        => [.. assembly
            .GetTypes()
            .Where(t => typeof(CodebookRecordType).IsAssignableFrom(t) && !t.IsAbstract)];
}
