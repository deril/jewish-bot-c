FROM microsoft/aspnetcore:2.0.0-jessie

COPY bin/Debug/netcoreapp2.0/publish /app

WORKDIR /app

ENV ASPNETCORE_URLS http://+:5000

EXPOSE 5000

ENTRYPOINT ["dotnet", "JewishBot.dll"]
