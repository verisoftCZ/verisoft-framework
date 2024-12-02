#Dokumentace

- Controllery dokumentujeme pomocí xml komentářů
- Dokumentují se všechny veřejné metody v controllerech

Pro funkčnost je potřeba vygenerovat .xml soubor s komentáři.
Do .csproj (Host) souboru s controllery je potřeba přidat:

	
``` xml
<PropertyGroup>
	<GenerateDocumentationFile>true</GenerateDocumentationFile> // zajistí vygenerování souboru
	<NoWarn>$(NoWarn);1591</NoWarn>                             // odstraní warningy u všech public metod, které nemají komentáře
</PropertyGroup>
```

Následně je potřeba .xml soubor zaregistrovat a povolit xml dokumentaci.

V Program.cs:

        
```  csharp
serviceCollection.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            // ... rest of AddSwaggerGen options
        });
```
Poté již můžeme přidávat komentáře nad controllery, které by měly popsat co controller vrací a jaké jsou vstupní parametry metody.

``` csharp
/// <summary>
/// Returns detail of Client with requested id.
/// </summary>
/// <remarks>
/// The HTTP GET method, that returns detailed information about client with certain id.
/// </remarks>
/// <param name="id">
/// Unique identifier of client by which record is got.
/// </param>
/// <returns>Returns data in JSON format</returns>
[HttpGet]
[Route("{id}")]

[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
public async Task<IActionResult> GetClientAsync(int id)
{
    var result = await clientService.GetClientAsync(id);
    return result.ToActionResult();
}
```
