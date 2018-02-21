set projectDirectory=%1
set buildType=%2

copy /y "%projectDirectory%\bin\%buildType%\netstandard1.6\SQLDataAccess.dll", "%projectDirectory%\..\Api\bin\%buildType%\netcoreapp1.0"
