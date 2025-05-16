FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src

COPY **/*.csproj ./
RUN for file in $(find . -name "*.csproj"); do mkdir -p $(dirname $file); done
RUN for file in $(find . -name "*.csproj"); do mv $file $(dirname $file)/; done

RUN dotnet restore "Bipki.App/Bipki.App.csproj"
COPY . .
WORKDIR "/src/Bipki.App"
RUN dotnet build "Bipki.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Bipki.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bipki.App.dll"]
