FROM microsoft/dotnet:2.2-aspnetcore-runtime

COPY bin/Release/netcoreapp2.2/publish /app

WORKDIR /app

ENV ASPNETCORE_URLS http://+:5000

EXPOSE 5000

ENTRYPOINT ["dotnet", "JewishBot.dll"]
