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
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Application/Verisoft.DemoApi.Application.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Application/"]
COPY ["Verisoft.Core/Verisoft.Core.TypeMapper/Verisoft.Core.TypeMapper.csproj", "Verisoft.Core/Verisoft.Core.TypeMapper/"]
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Contracts/Verisoft.DemoApi.Contracts.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Contracts/"]
COPY ["Verisoft.DemoApi/Verisoft.DemoApi.Common/Verisoft.DemoApi.Common.csproj", "Verisoft.DemoApi/Verisoft.DemoApi.Data/"]
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