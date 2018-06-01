FROM microsoft/dotnet:2.1-aspnetcore-runtime

COPY bin/Release/netcoreapp2.1/publish /app

WORKDIR /app

ENV ASPNETCORE_URLS http://+:5000

EXPOSE 5000

ENTRYPOINT ["dotnet", "JewishBot.dll"]
