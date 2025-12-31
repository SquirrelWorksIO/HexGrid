# Run Tests with Coverage and Generate Reports
Write-Host "Running HexGrid Tests with Coverage..." -ForegroundColor Cyan

# Clean old results
if (Test-Path "TestResults") {
    Write-Host "Cleaning old test results..." -ForegroundColor Yellow
    Remove-Item -Recurse -Force "TestResults\*" -ErrorAction SilentlyContinue
}

# Run tests with coverage
Write-Host "`nExecuting tests..." -ForegroundColor Green
dotnet test HexGrid.Tests/HexGrid.Tests.csproj --collect:"XPlat Code Coverage" --results-directory:./TestResults --logger "trx;LogFileName=TestResults.trx" --logger "console;verbosity=normal"

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nTests failed!" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Check if reportgenerator is installed
$reportGenExists = Get-Command reportgenerator -ErrorAction SilentlyContinue
if (-not $reportGenExists) {
    Write-Host "`nInstalling ReportGenerator..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-reportgenerator-globaltool
}

# Generate coverage report
Write-Host "`nGenerating coverage report..." -ForegroundColor Green
reportgenerator -reports:"TestResults\**\coverage.cobertura.xml" -targetdir:"TestResults\CoverageReport" -reporttypes:"Html;TextSummary;Badges"

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nReport generation failed!" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Success
Write-Host "`nTest execution completed successfully!" -ForegroundColor Green
Write-Host "`nCoverage Reports Generated:" -ForegroundColor Cyan
Write-Host "   HTML Report: TestResults\CoverageReport\index.html" -ForegroundColor White
Write-Host "   Text Summary: TestResults\CoverageReport\Summary.txt" -ForegroundColor White
Write-Host "   TRX Results: TestResults\TestResults.trx" -ForegroundColor White

# Display summary
if (Test-Path "TestResults\CoverageReport\Summary.txt") {
    Write-Host "`nCoverage Summary:" -ForegroundColor Cyan
    Get-Content "TestResults\CoverageReport\Summary.txt" | Select-Object -First 20
}

# Offer to open the HTML report
Write-Host "`nOpen HTML coverage report in browser? (Y/N): " -NoNewline
$openReport = Read-Host
if ($openReport -eq 'Y' -or $openReport -eq 'y') {
    Invoke-Item "TestResults\CoverageReport\index.html"
}
