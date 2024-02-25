FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/apps/Caracal.Device.Manager/Caracal.Device.Manager.csproj", "src/apps/Caracal.Device.Manager/"]
COPY ["src/libs/update-managers/Caracal.Device.UpdateManager.Hawkbit.Rest/Caracal.Device.UpdateManager.Hawkbit.Rest.csproj", "src/libs/update-managers/Caracal.Device.UpdateManager.Hawkbit.Rest/"]
COPY ["src/libs/update-managers/Caracal.Device.UpdateManager/Caracal.Device.UpdateManager.csproj", "src/libs/update-managers/Caracal.Device.UpdateManager/"]
COPY ["src/libs/update-managers/Caracal.Device.UpdateManager.Hawkbit.RabbitMQ/Caracal.Device.UpdateManager.Hawkbit.RabbitMQ.csproj", "src/libs/update-managers/Caracal.Device.UpdateManager.Hawkbit.RabbitMQ/"]
COPY ["src/libs/Caracal.SoftwareUpdate.Application/Caracal.SoftwareUpdate.Application.csproj", "src/libs/Caracal.SoftwareUpdate.Application/"]
COPY ["src/libs/Caracal.Messaging.Mqtt/Caracal.Messaging.Mqtt.csproj", "src/libs/Caracal.Messaging.Mqtt/"]
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
