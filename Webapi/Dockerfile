FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime

LABEL version="1.0" maintainer="Dyego Albuquerque"

WORKDIR  /app

COPY  ./dist .

ENTRYPOINT  ["dotnet", "Webapi.dll"]