using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Principal;
using System.Text;
using Verisoft.Core.Authentication;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Entities.CodebookRecords;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Host.InstallExtensions;

public static class SeedDataExtensions
{
    private const string SeedDataEnabledKey = "SeedData:Enabled";
    private const string SeededUserApiKey = "XXX";
    private const string AccessibilityStringValue = "Public";

    private static readonly (string FirstName, string Surname)[] SeedUserNames =
    [
        ("Jan", "Novák"), ("Marie", "Veselá"), ("Petr", "Dvořák"), ("Jana", "Němcová"),
        ("Jiří", "Svoboda"), ("Eva", "Pospíšilová"), ("Martin", "Procházka"), ("Hana", "Sedláčková"),
        ("Tomáš", "Kučera"), ("Anna", "Jelínková"), ("Josef", "Horák"), ("Lenka", "Růžičková"),
        ("Pavel", "Pokorný"), ("Kateřina", "Bartošová"), ("Miroslav", "Marek"), ("Věra", "Musilová"),
        ("Zdeněk", "Hájek"), ("Alena", "Doležalová"), ("František", "Král"), ("Petra", "Königová"),
        ("Václav", "Beneš"), ("Lucie", "Veselá"), ("Michal", "Fiala"), ("Markéta", "Němcová"),
        ("Jaroslav", "Krejčí"), ("Zuzana", "Pospíšilová"), ("Karel", "Zeman"), ("Ivana", "Sedláčková"),
        ("Lukáš", "Kolář"), ("Barbora", "Jelínková"), ("David", "Novák"), ("Martina", "Růžičková"),
        ("Ondřej", "Dvořák"), ("Veronika", "Bartošová"), ("Radek", "Svoboda"), ("Michaela", "Musilová"),
        ("Vladimír", "Procházka"), ("Simona", "Doležalová"), ("Daniel", "Kučera"), ("Kristýna", "Königová"),
        ("Adam", "Horák"), ("Filip", "Pokorný"), ("Matěj", "Marek"), ("Jakub", "Hájek"),
        ("Vojtěch", "Král"), ("Šimon", "Beneš"), ("Dominik", "Fiala"), ("Aleš", "Krejčí"),
        ("Roman", "Zeman"), ("Stanislav", "Kolář"),
    ];

    public static void SeedDemoData(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
    {
        if (!configuration.GetValue<bool>(SeedDataEnabledKey))
        {
            return;
        }

        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DemoApiSeeder");
        var dbContext = scope.ServiceProvider.GetRequiredService<IDemoApiDbContext>();
        var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();

        userContext.SetUser(new GenericPrincipal(new GenericIdentity("Seeder"), []));

        try
        {
            EnsureSeedData(dbContext, logger);
        }
        finally
        {
            userContext.ClearInjectedUserAndToken();
        }
    }

    private static void EnsureSeedData(IDemoApiDbContext dbContext, ILogger logger)
    {
        var codebookSet = dbContext.Set<CodebookRecordEntity<Accessibility>>();
        var translationSet = dbContext.Set<CodebookRecordTranslationEntity<Accessibility>>();

        var accessibility = codebookSet
            .Include(x => x.Translations)
            .FirstOrDefault(x => x.StringValue == AccessibilityStringValue);

        if (accessibility is null)
        {
            accessibility = new CodebookRecordEntity<Accessibility>
            {
                StringValue = AccessibilityStringValue,
                Description = "Seeded accessibility",
                ValidFrom = DateOnly.FromDateTime(DateTime.UtcNow),
            };

            codebookSet.Add(accessibility);
            dbContext.Commit();
            logger.LogInformation("Seeded codebook accessibility '{StringValue}'.", AccessibilityStringValue);
        }

        var hasAccessibilityTranslation = translationSet.Any(x =>
            x.CodebookRecordId == accessibility.Id &&
            x.LanguageCode == "en");

        if (!hasAccessibilityTranslation)
        {
            translationSet.Add(new CodebookRecordTranslationEntity<Accessibility>
            {
                LanguageCode = "en",
                Value = "Public",
                CodebookRecordId = accessibility.Id,
                CodebookRecord = accessibility,
            });

            dbContext.Commit();
            logger.LogInformation("Seeded codebook accessibility translation for language 'en'.");
        }

        var user = dbContext.User.FirstOrDefault();
        if (user is null)
        {
            for (var i = 0; i < SeedUserNames.Length; i++)
            {
                var (firstName, surname) = SeedUserNames[i];
                var newUser = new UserEntity
                {
                    FirstName = firstName,
                    Surname = surname,
                    Email = $"{RemoveDiacritics(firstName)}.{RemoveDiacritics(surname)}@verisoft-demo.test".ToLowerInvariant(),
                    ApiKey = i == 0 ? SeededUserApiKey : $"seed-{Guid.NewGuid():N}",
                    PasswordHash = "seed-password-hash",
                    Salt = "seed-salt",
                };

                dbContext.User.Add(newUser);
                user ??= newUser;
            }

            dbContext.Commit();
            logger.LogInformation("Seeded {Count} demo users.", SeedUserNames.Length);
        }

        var client = dbContext.Client.FirstOrDefault();
        if (client is null)
        {
            client = new ClientEntity
            {
                Name = "Seed Client",
                Representative = "Seed Representative",
            };

            dbContext.Client.Add(client);
            dbContext.Commit();
            logger.LogInformation("Seeded one client row.");
        }

        if (!dbContext.Document.Any())
        {
            dbContext.Document.Add(new DocumentEntity
            {
                Name = "Seed Document",
                Description = "Seeded document",
                UserId = user!.Id,
                ClientId = client.Id,
                ContentType = "text/plain",
                ValidFrom = DateTime.UtcNow,
            });

            dbContext.Commit();
            logger.LogInformation("Seeded one document row.");
        }

        if (!dbContext.CodebookParent.Any())
        {
            dbContext.CodebookParent.Add(new CodebookParentEntity
            {
                Code = "SEED-CBP-001",
                Name = "Seed Codebook Parent",
                Accessibility = Accessibility.Public,
                ClientId = client.Id,
            });

            dbContext.Commit();
            logger.LogInformation("Seeded one codebook parent row linked to accessibility '{StringValue}'.", AccessibilityStringValue);
        }
    }

    private static string RemoveDiacritics(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(c);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
