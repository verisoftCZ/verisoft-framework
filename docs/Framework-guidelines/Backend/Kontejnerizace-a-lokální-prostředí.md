# Lokální prostředí

Pro vývoj aplikací využíváme kontejnerizaci i pro aplikace, které nutné nemusí nutně běžet na linux prostředí. Tato izolace do kontejnerů nám umožňuje spouštět aplikace v různých prostředích a stavět modulární a nezávislé jednotky (mikroservisy).

Kontejnerizaci provádíme pomocí DockerFile souborů pro jednotlivé projekty.
Pro spuštění celého prostředí je nutné vytvořit docker-compose file.

pro spuštění je nutné zadat: `docker-compose up`

docker-compose.yml

``` yaml
version: '3.4'

services:
  api:
    image: ${DOCKER_REGISTRY}verisoft/verisoft_framework/demoapi
    build:
      context: ./src
      dockerfile: Verisoft.DemoApi/Verisoft.DemoApi.Host/Dockerfile
    depends_on:
      - db
    environment:
      - ASPNETCORE_ENVIRONMENT=DevelopmentDocker
      - ConnectionStrings__DefaultConnection=Server=db;Database=VerisoftFramework;User Id=sa;Password=your.password!;MultiSubnetFailover=True;TrustServerCertificate=True;Encrypt=False;Persist Security Info=True;

  frontend:
     image: ${DOCKER_REGISTRY}verisoft/verisoft_framework/app
     build:
       context: ./src/Verisoft.Frontend
       dockerfile: Dockerfile
     depends_on:
       - api

  db:
    image: ${DOCKER_REGISTRY}verisoft/verisoft_framework/db
    build:
      context: ./deploy/db
      dockerfile: Dockerfile
    ports:
      - 1433:1433

  router:
     image: ${DOCKER_REGISTRY}verisoft/verisoft_framework/router
     build:
       context: ./deploy/router
       dockerfile: Dockerfile
     ports:
       - 80:80
     depends_on:
       - api
       - frontend
```
V rámci standadního prostředí si spouštíme všechny komponenty systému. To znamená api (zde demoapi), frontend (frontend pro demoapi) a db (db pro demoapi). Router je obecná komponenta zajišťující správné routování na lokálním prostředí. Router funguje jako proxy pro jednolivé mikroservisy.

Pro všechny mikroservisy je nutné napsat DockerFiles, které z jednotlivých částí udělají kontejnery, které je pak možné spustit v clusteru.

DockerFile pro DemoAPI (složitější příklad, jelikož DemoApi využívá Pupeteer)

``` docker
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

#####################
#PUPPETEER RECIPE based on https://github.com/hardkoded/puppeteer-sharp/issues/1180#issuecomment-1015532968
#####################
RUN apt-get update && apt-get -f install && apt-get -y install wget gnupg2 apt-utils
RUN wget -q -O - https://dl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN echo 'deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main' >> /etc/apt/sources.list
RUN apt-get update \
&& apt-get install -y google-chrome-stable --no-install-recommends --allow-downgrades fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf
######################
#END PUPPETEER RECIPE
######################
ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/google-chrome-stable"

USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Host/Verisoft.DemoApi.Host.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Host/"]
COPY ["Verisoft.Core/Verisoft.Core.AspNet/Verisoft.Core.AspNet.csproj", "Verisoft.Core/Verisoft.Core.AspNet/"]
COPY ["Verisoft.Core/Verisoft.Core.Authentication/Verisoft.Core.Authentication.csproj", "Verisoft.Core/Verisoft.Core.Authentication/"]
COPY ["Verisoft.Core/Verisoft.Core.Common/Verisoft.Core.Common.csproj", "Verisoft.Core/Verisoft.Core.Common/"]
COPY ["Verisoft.Core/Verisoft.Core.Contracts/Verisoft.Core.Contracts.csproj", "Verisoft.Core/Verisoft.Core.Contracts/"]
COPY ["Verisoft.Core/Verisoft.Core.Entity/Verisoft.Core.Entity.csproj", "Verisoft.Core/Verisoft.Core.Entity/"]
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Application/Verisoft.DemoApi.Application.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Application/"]
COPY ["Verisoft.Core/Verisoft.Core.TypeMapper/Verisoft.Core.TypeMapper.csproj", "Verisoft.Core/Verisoft.Core.TypeMapper/"]
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Contracts/Verisoft.DemoApi.Contracts.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Contracts/"]
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Data/Verisoft.DemoApi.Data.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Data/"]
COPY ["Verisoft.Core/Verisoft.Core.Data.EF/Verisoft.Core.Data.EntityFramework.csproj", "Verisoft.Core/Verisoft.Core.Data.EF/"]
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Data.EF/Verisoft.DemoApi.Data.EF.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Data.EF/"]
COPY ["Verisoft.Core/Verisoft.Core.Data/Verisoft.Core.Data.csproj", "Verisoft.Core/Verisoft.Core.Data/"]
RUN dotnet restore "./Verisoft.DemoApi/Verisoft.DemoApi.Host/./Verisoft.DemoApi.Host.csproj"
COPY . .
WORKDIR "/src/Verisoft.DemoApi/Verisoft.DemoApi.Host"
RUN dotnet build "./Verisoft.DemoApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Verisoft.DemoApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
ENTRYPOINT ["dotnet", "Verisoft.DemoApi.Host.dll"]
```

Nejlehčí varianta DemoApi bez core projektů:
``` docker
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Host/Verisoft.DemoApi.Host.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Host/"]
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Application/Verisoft.DemoApi.Application.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Application/"]
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Contracts/Verisoft.DemoApi.Contracts.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Contracts/"]
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Data/Verisoft.DemoApi.Data.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Data/"]
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Data.EF/Verisoft.DemoApi.Data.EF.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Data.EF/"]
RUN dotnet restore "./Verisoft.DemoApi/Verisoft.DemoApi.Host/./Verisoft.DemoApi.Host.csproj"
COPY . .
WORKDIR "/src/Verisoft.DemoApi/Verisoft.DemoApi.Host"
RUN dotnet build "./Verisoft.DemoApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Verisoft.DemoApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
ENTRYPOINT ["dotnet", "Verisoft.DemoApi.Host.dll"]
```
od .net 8 pokud chceme dělat privileged akce jako změna portu tak je nutné zajistit i tuto funkcionalitu v nastavení .csproj:
	
``` xml
<PropertyGroup>
	<previous settings>
	<DockerDefaultTargetOS>Linux</DockerDefaultTargetOS>
	<DockerfileContext>..\..</DockerfileContext>
	<EnableSdkContainerSupport>true</EnableSdkContainerSupport>
	<ContainerUser>root</ContainerUser>
</PropertyGroup>
```

Pro DB je nutné vytvořit inicializační skript o ostaní se starají migrace a Seed metody. //TODO odkaz na DB ve wiki
DockerFile

``` docker
FROM mcr.microsoft.com/mssql/server
ENV SA_PASSWORD=.share8!klaus
ENV ACCEPT_EULA=Y

COPY ./*.sql /tmp/sql/

RUN ( /opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" \
    && /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P ${SA_PASSWORD} -d master -i /tmp/sql/01-init.sql\
    && pkill sqlservr
```
Init script

``` sql
CREATE DATABASE [VerisoftFramework];  --Change to microservice DBName
GO;

USE [VerisoftFramework];  --Change to microservice DBName
```

Router:


``` docker
FROM nginx:stable-alpine
COPY nginx.conf /etc/nginx/nginx.conf
```

Nastavení router: (nginx.conf)

```
user  nginx;
worker_processes  auto;

error_log  /var/log/nginx/error.log notice;
pid        /var/run/nginx.pid;


events {
    worker_connections  1024;
}

http {
    include       /etc/nginx/mime.types;
    default_type  application/octet-stream;
    client_max_body_size 100M;

    log_format  main  '$remote_addr - $remote_user [$time_local] "$request" '
                      '$status $body_bytes_sent "$http_referer" '
                      '"$http_user_agent" "$http_x_forwarded_for"';

    access_log  /var/log/nginx/access.log  main;

    sendfile        on;
    #tcp_nopush     on;

    keepalive_timeout  65;
    
    upstream api_server {
       # server host.docker.internal:5000;
       server api:80;
    }
    
    upstream frontend_server {
        server frontend:80;
    }

    server {
        listen       80;
        listen  [::]:80;
        server_name  localhost;
        client_max_body_size 100M;

        error_page   500 502 503 504  /50x.html;
        location = /50x.html {
            root   /usr/share/nginx/html;
        }

        location /api/ {
           proxy_pass http://api_server/;
           proxy_set_header   Upgrade $http_upgrade;
           proxy_set_header   Connection keep-alive;
           proxy_set_header   Host $host;
           proxy_cache_bypass $http_upgrade;
           proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
           proxy_set_header   X-Forwarded-Proto $scheme;
           client_max_body_size 100M;
        }

        location / {
           proxy_pass http://frontend;
        }
    }
}
```