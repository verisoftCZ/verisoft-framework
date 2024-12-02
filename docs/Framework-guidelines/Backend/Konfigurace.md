# Appsettings

Appsettings dělíme podle prostředí.
Příklad:
**appsettings.json**
appsettings.**Local**.json - lokální prostředí pro docker-compose
appsettings.**DEV**.json
appsettings.**DEVExternal**.json - stejný jako DEV, ale dev poižívá interní adresy, zde používám například public IP pro DB jinak bych se na ní nedostal
appsettings.**TEST**.json
appsettings.**PROD**.json

obecné appsetitngs:
``` js
//může obsahovat i další, pouze ilustrativní příklad
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information"
    }
  },
  "HangfireJobs": {
    "JobOneSettings": "0 0 * * *",
    "JobTwoSettings": "0 1 * * *"
  },
  "AllowedHosts": "*",
  "apiSecret": "KEY_TO_DECODE_JWT_TOKEN"
}
```

pro specifické prostředí pak používáme konkrétní settings:

``` js
{
  "Database": {
    "ConnectionString": "CONNECTION_STRING_FOR_DEV"
  },
  "Kestrel": {
    "EndPoints": {
      "Http": {
        "Url": "http://0.0.0.0:80"
      }
    }
  }
}
```
pokud nechceme uchovávat citlivé konfigurace v appsettings.json je možné využít Azure Key Vault:
Předpokladem je založený Azure Key Vault + App registrace v azure portále
![image.png](/.attachments/image-c3ef00de-18e6-431b-adeb-58197d9556e6.png)
![image.png](/.attachments/image-ff8152fe-e84b-425e-8db0-6914142605f6.png)

Po založení key vault a app registrace je možné propojení key vaultu a aplikace v nastavení key vault.

V .net aplikaci je pak možné číst key vault secrets, stejně jako kdyby šlo a hodnoty v appsettings:
Například výše uvedený Database.ConnectionString bude v KV zapsán takto:
![image.png](/.attachments/image-5921e8d9-087a-47b7-9a2e-268113cdc61a.png)

v Program.cs je nutné zaregistorvat KV:

``` csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddAzureKeyVault(builder.Configuration);
```

v appsettings.json:
``` js
  "KeyVault": {
    "Uri": "https://verisoft-internal-vault.vault.azure.net/",
  },
  "AzureAd": {
    "TenantId": "<TenantId>",
    "ClientId": "<ClientId>",
  }
```

Pro spojení je nutné ještě definovat ClientSecret. Ideálně by neměl být uložen v appsettings čím opět vystavujeme citlivé údaje.
Takže doporučuji si ho nastavit do například do env variables:
![image.png](/.attachments/image-202bdb35-9b99-47ae-ae17-5f1e3c5ee453.png)
