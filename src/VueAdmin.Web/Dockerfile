#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
FROM net6-gdi:1.1

WORKDIR /app
COPY . /app

ENV TZ Asia/Shanghai
EXPOSE 80
ENTRYPOINT ["dotnet", "VueAdmin.Web.dll"]
