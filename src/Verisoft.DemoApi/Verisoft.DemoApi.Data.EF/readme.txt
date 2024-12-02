Prace ve VS: 
1. spustit: Developer powersheel
2. cd .\Verisoft.DemoApi\Verisoft.DemoApi.Database\


MIGRACE
dotnet ef --startup-project ..\Verisoft.DemoApi.Host migrations add AddedUserTable

UPDATE DB
$env:ASPNETCORE_ENVIRONMENT='DevelopmentDocker'
dotnet ef --startup-project ..\Verisoft.DemoApi.Host database update