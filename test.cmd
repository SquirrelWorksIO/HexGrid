@echo off
REM Run Tests with Coverage and Generate Reports

echo Running HexGrid Tests with Coverage...
echo.

REM Clean old results
if exist TestResults (
    echo Cleaning old test results...
    rd /s /q TestResults 2>nul
)

REM Run tests with coverage
echo Executing tests...
dotnet test HexGrid.Tests/HexGrid.Tests.csproj --collect:"XPlat Code Coverage" --results-directory:./TestResults --logger "trx;LogFileName=TestResults.trx" --logger "console;verbosity=normal"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Tests failed!
    exit /b %ERRORLEVEL%
)

REM Check if reportgenerator exists
where reportgenerator >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Installing ReportGenerator...
    dotnet tool install --global dotnet-reportgenerator-globaltool
)

REM Generate coverage report
echo.
echo Generating coverage report...
reportgenerator -reports:"TestResults\**\coverage.cobertura.xml" -targetdir:"TestResults\CoverageReport" -reporttypes:"Html;TextSummary;Badges"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✓ Test execution completed successfully!
    echo.
    echo Coverage Reports Generated:
    echo    HTML Report: TestResults\CoverageReport\index.html
    echo    Text Summary: TestResults\CoverageReport\Summary.txt
    echo    TRX Results: TestResults\TestResults.trx
    echo.
    type TestResults\CoverageReport\Summary.txt 2>nul | more /E +0
) else (
    echo.
    echo ✗ Report generation failed!
    exit /b %ERRORLEVEL%
)
