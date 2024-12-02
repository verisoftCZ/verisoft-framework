
#Endpointy a routing
Standardní controller musí mít 5 základních endpointů
- **POST** - založení nového záznamu
apiUrl/<NázevControlleru>

``` csharp
[HttpPost]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionErrors))]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<Customer>))]
public async Task<IActionResult> AddCustomerAsync(CustomerCreateModel customerCreateModel)
{
   var result = await customerService.AddCustomerAsync(customerCreateModel);
   return result.ToActionResult();
}
```

- **PUT** - update stávajícího záznamu
apiUrl/<NázevControlleru>/{id}
``` csharp
[HttpPut]
[Route("{id}")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionErrors))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<Customer>))]
public async Task<IActionResult> UpdateCustomerAsync(CustomerEditModel customerEditModel, int id)
{
   var result = await customerService.UpdateCustomerAsync(customerEditModel, id);
   return result.ToActionResult();
}
```
- **DELETE** - smazání stávajícího záznamu
apiUrl/<NázevControlleru>/{id}
``` csharp
[HttpDelete]
[Route("{id}")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
public async Task<IActionResult> DeleteCustomerAsync(int id)
{
   var result = await customerService.DeleteCustomerAsync(id);
   return result.ToActionResult();
}
```
- **GET** - získání jednoho záznamu na základě primárního klíče
apiUrl/<NázevControlleru>/{id}
-- v případě, že objekt s daným id nebyl nalezen vrací se 404

``` csharp
[HttpGet]
[Route("{id}")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
public async Task<IActionResult> GetCustomerAsync(int id)
{
   var result = await customerService.GetCustomerAsync(id);
   return result.ToActionResult();
}
```
- **GET** - získání filtrovaných paged dat pro list záznamů (hlavně pro vykreslení do FE tabulky)
apiUrl/<NázevControlleru>?queryParams
   
``` csharp
[HttpGet]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerListData))]
public async Task<CustomertListData> GetCustomerListDataAsync([FromQuery] FilteredRequest<CustomerFilter> request)
{
   return await customerService.GetCustomerListDataAsync(request);
}
```
Routing dalších endpointů je hierarchický, tedy např. apiUrl/Customer/{id}/AssignToGroup/{groupId}

TODO Export endpoint @<9AF2437A-7AD6-6473-8D2C-9A8ECC3D68E5> 