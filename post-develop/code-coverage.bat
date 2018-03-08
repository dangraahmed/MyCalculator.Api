mkdir "test\ApiTest\CodeCoverage"

%userprofile%\.nuget\packages\OpenCover\4.6.519\tools\OpenCover.Console.exe ^
    -target:"%ProgramFiles%\dotnet\dotnet.exe" ^
    -targetargs:"test test\ApiTest" ^
    -register:user ^
    -filter:"+[*]* -[xunit*]* -[Moq*]*" ^
    -output:"test\ApiTest\CodeCoverage\ApiTest.xml"^
    -oldStyle

%userprofile%\.nuget\packages\ReportGenerator\3.1.2\tools\ReportGenerator.exe ^
    -reports:"test\ApiTest\CodeCoverage\ApiTest.xml" ^
    -targetdir:"test\ApiTest\CodeCoverage"

start "report" "test\ApiTest\CodeCoverage\index.htm"
cls