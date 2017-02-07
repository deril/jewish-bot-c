FROM microsoft/aspnetcore:1.1.0

COPY bin/Debug/netcoreapp1.1/publish /app

WORKDIR /app

ENV ASPNETCORE_URLS http://+:5000

EXPOSE 5000

ENTRYPOINT ["dotnet", "JewishBot.dll"]
