FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
WORKDIR /app
ADD bin/Release/net8.0/publish .
ENTRYPOINT ["dotnet","Azure.Containers.WebApi.HelloWorld.dll"]