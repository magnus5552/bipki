# Docs

Документация [здесь](https://docs.google.com/document/d/1xo8Aq9cncICgDXO6kTGeE1dpUakcCxkNt-iC9PnJ2U8/edit?usp=sharing)

# Usage

В корне репозитория

- `docker compose up postgres -d`
- `dotnet publish -c Release Bipki.App/Bipki.App.csproj`
- `./Bipki.App/bin/Release/net8.0/publish/Bipki.App.exe`

Сервис будет доступен на порту 5000

# Settings

В Bipki.App/appsettings.json две настройки:

```json
"DatabaseOptions": {
    "DbConnectionString": "строка подключения к postgres"
},
"AuthorizationOptions": {
  "AdminToken": "токен для авторизации организаторов"
}
```
