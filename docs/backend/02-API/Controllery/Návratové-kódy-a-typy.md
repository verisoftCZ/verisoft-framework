#Návratové kódy a typy
Na controlleru pomocí atributů nastavím společné návratové kódy.
``` csharp
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType<UnauthorizedResult>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<BadRequestResult>(StatusCodes.Status404BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CustomerController(ICustomerService customerService)
{
    // ... endpoints
}
```
- na jednotlivých endpointech specifikuji návratové kódy, které se od controlleru odchylují
- pokud endpoint vrací něco jiného než 200, je návratový typ metody IActionResult
-- IActionResult získáme pomocí extension metody ToActionResult pro Core.Contracts.BusinessResult.BusinessActionResult<TResult> alternativně Core.Contracts.BusinessResult.BusinessActionResult
- v ProducesResponseType atributu se generický parametr nastaví na 
-- **200** - contract, který je výsledkem úspěšného volání service
-- **422** - Core.Contracts.BusinessResult.BusinessActionErrors<contract z předchozí odrážky>
-- **Vše ostatní** - Core.Contracts.BusinessResult.BusinessActionErrors

``` csharp
[HttpPut]
[Route("{id}")]
[ProducesResponseType<Customer>(StatusCodes.Status200OK)]
[ProducesResponseType<BusinessActionErrors>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
[ProducesResponseType<BusinessActionErrors<Customer>>(StatusCodes.Status422UnprocessableEntity)]
public async Task<IActionResult> UpdateCustomerAsync(CustomerEditModel customerEditModel, int id)
{
   var result = await customerService.UpdateCustomerAsync(customerEditModel, id);
   return result.ToActionResult();
}
```
**ToActionResult()**
Extension metoda nad Core.Common.BusinessResult.BusinessActionResult<TResult>
- Pokud je naplněna property Data objektem, který je implementací interface IValidatedObject a tento objekt má v property ValidationErrors nějaký element => UnprocessableEntityObjectResult(new BusinessActionErrors<T>(Data, Errors)) **(422)**
- Pokud je naplněna property Data => OkObjectResult(Data) **(200)**
- Pokud je v property Errors první Error s Code = BusinessResultCodes.EntityNotFound => NotFoundObjectResult(Errors) **(404)**
- Pokud je v property Errors první Error s Code = BusinessResultCodes.ForbiddenErrorCode => ForbiddenObjectResult(Errors) **(403)**
- Pokud je v property Errors první Error s Code = BusinessResultCodes.BadRequestErrorCode => BadRequestObjectResult(Errors) **(400)**
- Pokud je v property Errors první Error s Code = BusinessResultCodes.NullInputErrorCode => BadRequestObjectResult(Errors) **(400)**
- Ostatní případy => UnprocessableEntityObjectResult(Errors) **(422)**

Metoda se switchem, který generuje IActionResult:
``` csharp
private static IActionResult GetErrorResult<T>(string errorCode, BusinessActionResult<T> result, IDictionary<string, IActionResult> customCases)
{
    var defaultCases = new Dictionary<string, IActionResult>
    {
            { ErrorFactory.NotFoundErrorCode, new NotFoundObjectResult(result.Errors) },
            { ErrorFactory.ForbiddenErrorCode, new ForbiddenObjectResult(result.Errors) },
            { ErrorFactory.BadRequestErrorCode, new BadRequestObjectResult(result.Errors) },
            { ErrorFactory.NullInputErrorCode, new BadRequestObjectResult(result.Errors) { ContentTypes = HttpProblemDetailsStandard() } },
    };

    var switchCases = customCases != null
        ? defaultCases.Concat(customCases).ToDictionary(kv => kv.Key, kv => kv.Value)
        : defaultCases;

    return switchCases.TryGetValue(errorCode, out var actionResult)
        ? actionResult
        : result.Data != null
            ? new UnprocessableEntityObjectResult(new BusinessActionErrors<T>(result.Data, result.Errors)) }
            : new UnprocessableEntityObjectResult(result.Errors) };
}
```
Při volání metody ToActionResult() mohu rozšířit switch o další http result kódy nebo o další Error kódy pomocí Dictionary:
        
``` csharp
[HttpPut]
[Route("{id}")]
[ProducesResponseType<Customer>(StatusCodes.Status200OK)]
[ProducesResponseType<BusinessActionErrors>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
[ProducesResponseType<BusinessActionErrors<Customer>>(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status202Accepted)]
[ProducesResponseType(StatusCodes.Status302Found)]
public async Task<IActionResult> UpdateCustomerAsync(CustomerEditModel customerEditModel, int id)
{
   var result = await customerService.UpdateCustomerAsync(customerEditModel, id);
   var customActionResults = new Dictionary<string, IActionResult>
   {
      ["CustomErrorCode"] = new RedirectResult("https://www.redirect-url.com"),
      ["CustomErrorCode2"] = new AcceptedResult(),
   };

   return result.ToActionResult(customActionResults);
}
```
Způsob, jak korektně do controlleru vrátit `BusinessActionResult`, je popsán v
článku [Validace](../Validace.md).