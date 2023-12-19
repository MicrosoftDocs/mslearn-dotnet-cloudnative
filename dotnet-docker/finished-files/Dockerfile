FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /DataEntities
COPY "DataEntities/DataEntities.csproj" .
RUN dotnet restore
COPY "DataEntities" .
RUN dotnet publish -c release -o /app

WORKDIR /src
COPY Products/Products.csproj .
RUN dotnet restore
COPY Products .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Products.dll"]