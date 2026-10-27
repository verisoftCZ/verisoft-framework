# NuGet balíčky

Backendové projekty používají interní knihovny `Verisoft.Core.*` jako
ProjectReference v rámci tohoto repozitáře. Při publikaci knihoven se řiďte
pipeline a feedem definovaným v `nuget.config`; přístup k privátnímu Azure
Artifacts feedu musí být nastavený mimo zdrojový kód.

Verze balíčků se mění v příslušném `.csproj`. Po změně proveďte restore, build a
relevantní testy. Do dokumentace ani repozitáře nepatří API klíče ani osobní
access tokeny.
