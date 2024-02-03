FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/apps/Caracal.Device.Manager/Caracal.Device.Manager.csproj", "src/apps/Caracal.Device.Manager/"]
RUN dotnet restore "src/apps/Caracal.Device.Manager/Caracal.Device.Manager.csproj"
COPY . .
WORKDIR "/src/src/apps/Caracal.Device.Manager"
RUN dotnet build "Caracal.Device.Manager.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Caracal.Device.Manager.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Caracal.Device.Manager.dll"]
