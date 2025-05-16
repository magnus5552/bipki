FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release


# Install Node.js
RUN apt-get update \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src

COPY *.sln ./
COPY */*.*proj ./

RUN dotnet sln list | \
      tail -n +3 | \
      xargs -I {} sh -c \
        'target="{}"; dir="${target%/*}"; file="${target##*/}"; mkdir -p -- "$dir"; mv -- "$file" "$target"'


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
