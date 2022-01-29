FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
LABEL org.opencontainers.image.source=https://github.com/deril/jewish-bot-c
WORKDIR /app
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY JewishBot.csproj ./
RUN dotnet restore ./JewishBot.csproj
COPY . .
WORKDIR /src/.
RUN dotnet build JewishBot.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish JewishBot.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JewishBot.dll"]
