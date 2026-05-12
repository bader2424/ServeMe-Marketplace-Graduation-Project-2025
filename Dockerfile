# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
WORKDIR /src
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
    NUGET_XMLDOC_MODE=skip
COPY ["SERVE_ME_PROJECT/SERVE_ME_PROJECT.csproj", "SERVE_ME_PROJECT/"]
RUN --mount=type=cache,id=serveme-nuget,target=/root/.nuget/packages \
    dotnet restore "SERVE_ME_PROJECT/SERVE_ME_PROJECT.csproj" --disable-parallel

FROM restore AS publish
COPY . .
WORKDIR /src/SERVE_ME_PROJECT
RUN --mount=type=cache,id=serveme-nuget,target=/root/.nuget/packages \
    dotnet publish "SERVE_ME_PROJECT.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_EnableDiagnostics=0
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SERVE_ME_PROJECT.dll"]
