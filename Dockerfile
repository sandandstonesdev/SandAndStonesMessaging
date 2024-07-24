FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY SandAndStonesMessagingSample/SandAndStonesMessagingSample.csproj SandAndStonesMessagingSample/
COPY SandAndStonesMessagingTests/SandAndStonesMessagingTests.csproj SandAndStonesMessagingTests/
RUN dotnet restore SandAndStonesMessagingSample/SandAndStonesMessagingSample.csproj
COPY . .
WORKDIR /src/SandAndStonesMessagingSample
RUN dotnet build SandAndStonesMessagingSample.csproj -c Release -o /app

FROM build AS testrunner
WORKDIR /src/SandAndStonesMessagingTests
CMD ["dotnet", "test", "--no-restore"]

FROM build AS publish
WORKDIR /src/SandAndStonesMessagingSample
FROM build AS publish
RUN dotnet publish SandAndStonesMessagingSample.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SandAndStonesMessagingSample.dll"]

