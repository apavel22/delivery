# OpenApi
```
openapi-generator generate -i https://gitlab.com/microarch-ru/microservices/dotnet/system-design/-/raw/main/services/delivery/contracts/openapi.yml -g aspnetcore -o ./Contract --package-name Api --additional-properties classModifier=abstract --additional-properties operationResultTask=true
openapi-generator-cli generate -g aspnetcore --additional-properties aspnetCoreVersion=3.1 --additional-properties classModifier=abstract --additional-properties operationResultTask=true --additional-properties buildTarget=library --additional-properties  operationModifier=abstract --additional-properties packageName=Api --additional-properties packageTitle=Api -i https://gitlab.com/microarch-ru/microservices/dotnet/system-design/-/raw/main/services/delivery/contracts/openapi.yml
```
# БД 
```
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
```
[Подробнее про dotnet cli](https://learn.microsoft.com/ru-ru/ef/core/cli/dotnet)

# Миграции
```
dotnet ef migrations add Init --startup-project ./DeliveryApp.Api --project ./DeliveryApp.Infrastructure
dotnet ef database update --startup-project ./DeliveryApp.Api --connection "Server=localhost;Port=5432;User Id=username;Password=secret;Database=delivery;"
```
# Docker
```
docker rm -vf $(docker ps -aq)
docker volume rm $(docker volume ls -q)
docker rmi -f $(docker images -aq)
```

# Docker Compose
```
docker-compose down --remove-orphans
docker-compose up
docker-compose up postgres
```
