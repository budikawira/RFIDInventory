###########
# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_ENVIRONMENT=Demo
# RUN apt-get update && apt-get install -y libgdiplus
# RUN apt-get install -y nano


COPY bin/Release/net8.0/publish/ App/
WORKDIR /App
ENV TZ="Asia/Jakarta"
ENTRYPOINT ["dotnet", "RfidBarcode.Crm.dll", "--server.urls", "http://+:80;https://+:443"]