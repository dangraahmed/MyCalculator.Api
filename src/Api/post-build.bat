set OutputPath=%1
set FullTargetFramework=%2
set Configuration=%3

dotnet publish-iis --publish-folder %OutputPath% --framework %FullTargetFramework%


copy /y "%OutputPath%\..\SQLDataAccess.dll", "%OutputPath%"
copy /y "%OutputPath%\..\MongoDataAccess.dll", "%OutputPath%"
copy /y "%OutputPath%\..\BusinessLogic.dll", "%OutputPath%"