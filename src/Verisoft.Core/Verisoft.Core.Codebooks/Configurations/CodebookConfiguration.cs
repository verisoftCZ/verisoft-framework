using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Codebooks.Context;

namespace Verisoft.Core.Codebooks.Configurations;

public static class CodebookConfiguration
{
    public static async Task EnsureSeedCodebookTableDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ICodebookDbContext>();
        var registry = scope.ServiceProvider.GetRequiredService<ICodebookRegistry>();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(nameof(CodebookConfiguration));
        var codebookTypes = registry.GetRegisteredCodebooks();
        if (codebookTypes.Count == 0)
        {
            return;
        }

        var codebookCodes = codebookTypes
            .Select(t => t.Name)
            .ToList();

        var existingCodebookCodes = await dbContext.Codebook
            .Select(p => p.Code)
            .ToListAsync();

        var missingCodes = codebookCodes.Except(existingCodebookCodes).Where(code => !string.IsNullOrWhiteSpace(code)).ToList();

        if (missingCodes.Count == 0)
        {
            return;
        }

        var newCodebooks = missingCodes.ConvertAll(x =>
        new CodebookEntity
        {
            Code = x!,
            Name = Regex.Replace(x, "(?<!^)(?=[A-Z])", " "),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            IsDeleted = false,
            IsExternal = false,
        });

        dbContext.Codebook.AddRange(newCodebooks);

        try
        {
            await ((CodebookDbContext)dbContext).SaveChangesAsync();
            logger.LogInformation("Successfully seeded {Count} new codebooks.", newCodebooks.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding codebooks.");
        }
    }
}
