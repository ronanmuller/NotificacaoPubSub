FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update
RUN apt-get install wget -y     #install wget lib

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "NotificacaoPubSub.Api/NotificacaoPubSub.Api.csproj" --configfile "NuGet.config" --disable-parallel
COPY . .
WORKDIR "/src/NotificacaoPubSub.Api"
RUN dotnet build "NotificacaoPubSub.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificacaoPubSub.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN wget -r https://s3.amazonaws.com/rds-downloads/rds-combined-ca-bundle.p7b  # download tar file to example folder and untar it in same folder
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificacaoPubSub.Api.dll"]