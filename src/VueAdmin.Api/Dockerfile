FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app
COPY . /app

ENV TZ Asia/Shanghai
EXPOSE 80
ENTRYPOINT ["dotnet", "VueAdmin.Api.dll"]