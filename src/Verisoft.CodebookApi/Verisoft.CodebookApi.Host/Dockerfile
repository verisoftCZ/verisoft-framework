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
COPY ["Verisoft.CodebookApi/Verisoft.CodebookApi.Host/Verisoft.CodebookApi.Host.csproj", "Verisoft.CodebookApi/Verisoft.CodebookApi.Host/"]
COPY ["Verisoft.Core/Verisoft.Core.AspNet/Verisoft.Core.AspNet.csproj", "Verisoft.Core/Verisoft.Core.AspNet/"]
COPY ["Verisoft.Core/Verisoft.Core.Authentication/Verisoft.Core.Authentication.csproj", "Verisoft.Core/Verisoft.Core.Authentication/"]
COPY ["Verisoft.Core/Verisoft.Core.Common/Verisoft.Core.Common.csproj", "Verisoft.Core/Verisoft.Core.Common/"]
COPY ["Verisoft.Core/Verisoft.Core.Contracts/Verisoft.Core.Contracts.csproj", "Verisoft.Core/Verisoft.Core.Contracts/"]
COPY ["Verisoft.CodebookApi/Verisoft.CodebookApi.Application/Verisoft.CodebookApi.Application.csproj", "Verisoft.CodebookApi/Verisoft.CodebookApi.Application/"]
COPY ["Verisoft.Core/Verisoft.Core.TypeMapper/Verisoft.Core.TypeMapper.csproj", "Verisoft.Core/Verisoft.Core.TypeMapper/"]
COPY ["Verisoft.CodebookApi/Verisoft.CodebookApi.Contracts/Verisoft.CodebookApi.Contracts.csproj", "Verisoft.CodebookApi/Verisoft.CodebookApi.Contracts/"]
COPY ["Verisoft.CodebookApi/Verisoft.CodebookApi.Core/Verisoft.CodebookApi.Core.csproj", "Verisoft.CodebookApi/Verisoft.CodebookApi.Core/"]
COPY ["Verisoft.Core/Verisoft.Core.Data.EF/Verisoft.Core.Data.EntityFramework.csproj", "Verisoft.Core/Verisoft.Core.Data.EF/"]
COPY ["Verisoft.CodebookApi/Verisoft.CodebookApi.Data.EF/Verisoft.CodebookApi.Data.EF.csproj", "Verisoft.CodebookApi/Verisoft.CodebookApi.Data.EF/"]
COPY ["Verisoft.Core/Verisoft.Core.Data/Verisoft.Core.Data.csproj", "Verisoft.Core/Verisoft.Core.Data/"]
RUN dotnet restore "./Verisoft.CodebookApi/Verisoft.CodebookApi.Host/./Verisoft.CodebookApi.Host.csproj"
COPY . .
WORKDIR "/src/Verisoft.CodebookApi/Verisoft.CodebookApi.Host"
RUN dotnet build "./Verisoft.CodebookApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Verisoft.CodebookApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
ENTRYPOINT ["dotnet", "Verisoft.CodebookApi.Host.dll"]