set projectDirectory=%1

copy /y "%projectDirectory%\bin\Debug\netstandard1.6\SQLDataAccess.dll", "%projectDirectory%\..\Api\bin\Debug\netcoreapp1.0"
