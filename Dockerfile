FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/MulliganDeck.Api/*.csproj src/MulliganDeck.Api/
COPY src/MulliganDeck.Domain/*.csproj src/MulliganDeck.Domain/
COPY src/MulliganDeck.Infrastructure/*.csproj src/MulliganDeck.Infrastructure/
RUN dotnet restore src/MulliganDeck.Api/MulliganDeck.Api.csproj

COPY . .
RUN dotnet publish src/MulliganDeck.Api/MulliganDeck.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "MulliganDeck.Api.dll"]