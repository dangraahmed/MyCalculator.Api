dotnet restore
dotnet build src\Core -c Release
dotnet build src\BusinessLogic -c Release
dotnet build src\Dto -c Release
dotnet build src\Api -c Release
dotnet build src\SQLDataAccess -c Release
dotnet build src\MongoDataAccess -c Release
