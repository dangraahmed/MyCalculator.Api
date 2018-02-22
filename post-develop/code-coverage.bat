mkdir "..\test\ApiTest\CodeCoverage"

C:\Users\ahmed.dangra\.nuget\packages\OpenCover\4.6.519\tools\OpenCover.Console.exe ^
    -target:"%ProgramFiles%\dotnet\dotnet.exe" ^
    -targetargs:"test ..\test\ApiTest" ^
    -register:user ^
    -filter:"+[*]* -[xunit*]*" ^
    -output:"..\test\ApiTest\CodeCoverage\ApiTest.xml"

C:\Users\ahmed.dangra\.nuget\packages\ReportGenerator\3.1.2\tools\ReportGenerator.exe ^
    -reports:"..\test\ApiTest\CodeCoverage\ApiTest.xml" ^
    -targetdir:"..\test\ApiTest\CodeCoverage"

start "report" "..\test\ApiTest\CodeCoverage\index.htm"
rem cls