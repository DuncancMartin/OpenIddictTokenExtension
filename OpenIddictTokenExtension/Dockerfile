FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY OpenIddictTokenExtension/OpenIddictTokenExtension.csproj OpenIddictTokenExtension/
COPY NuGet.config ./

RUN dotnet restore OpenIddictTokenExtension/OpenIddictTokenExtension.csproj
COPY . .

WORKDIR /src/OpenIddictTokenExtension
RUN dotnet build OpenIddictTokenExtension.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish OpenIddictTokenExtension.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OpenIddictTokenExtension.dll"]
