FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY JewishBot.csproj ./
RUN dotnet restore ./JewishBot.csproj
COPY . .
WORKDIR /src/.
RUN dotnet build JewishBot.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish JewishBot.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "JewishBot.dll"]
