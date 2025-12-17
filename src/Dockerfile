# Base image para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Expõe a porta que o Kestrel vai ouvir
EXPOSE 5000

# Força o ASP.NET Core a escutar em qualquer IP
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Docker

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "NotificacaoPubSub.Api/NotificacaoPubSub.Api.csproj" --disable-parallel
WORKDIR "/src/NotificacaoPubSub.Api"
RUN dotnet build "NotificacaoPubSub.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "NotificacaoPubSub.Api.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificacaoPubSub.Api.dll"]
