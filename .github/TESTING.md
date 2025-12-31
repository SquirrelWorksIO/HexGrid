# HexGrid Testing Guide

## Quick Start

### Running Tests with Coverage Reports

The project is configured to automatically generate coverage reports when tests are run.

#### Option 1: PowerShell Script (Recommended)
```powershell
.\test.ps1
```

This will:
- ✅ Run all tests
- ✅ Collect code coverage
- ✅ Generate HTML coverage report
- ✅ Generate text summary
- ✅ Show coverage statistics
- ✅ Offer to open HTML report in browser

#### Option 2: Batch File (Windows)
```cmd
test.cmd
```

Same functionality as PowerShell script, but for Command Prompt.

#### Option 3: VS Code Tasks
Press `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac) and select:
- **"Tasks: Run Test Task"** - Quick test run
- **"test-with-coverage"** - Run with full coverage reports
- **"test-watch"** - Watch mode (re-runs on file changes)
- **"open-coverage-report"** - Open latest HTML report

Or use the keyboard shortcut: `Ctrl+Shift+B` to run the default test task.

#### Option 4: Manual dotnet CLI
```bash
# Quick test
dotnet test HexGrid.Tests/HexGrid.Tests.csproj

# With coverage
dotnet test HexGrid.Tests/HexGrid.Tests.csproj --collect:"XPlat Code Coverage" --results-directory:./TestResults

# Generate report from coverage
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"TestResults/CoverageReport" -reporttypes:"Html;TextSummary"
```

## Generated Reports

After running tests with coverage, you'll find:

| Report | Location | Description |
|--------|----------|-------------|
| HTML Report | `TestResults/CoverageReport/index.html` | Interactive coverage report with drill-down |
| Text Summary | `TestResults/CoverageReport/Summary.txt` | Quick coverage statistics |
| TRX Results | `TestResults/TestResults.trx` | Detailed test execution results (XML) |
| Cobertura XML | `TestResults/**/coverage.cobertura.xml` | Machine-readable coverage data |
| Coverage Badges | `TestResults/CoverageReport/badge_*.svg` | SVG badges for README |

## Test Configuration

### Coverage Settings
Coverage collection is configured in:
- `HexGrid.Tests/test.runsettings` - Test execution settings
- `coverlet.runsettings.json` - Coverage filter configuration

### What's Included in Coverage
- ✅ All code in `HexGrid.Lib` namespace
- ❌ Test code is excluded
- ❌ Compiler-generated code is excluded
- ❌ Code marked with `[GeneratedCode]` or `[Obsolete]` is excluded

## Continuous Integration

For CI/CD pipelines, use:

```yaml
# Example GitHub Actions
- name: Test with Coverage
  run: dotnet test --collect:"XPlat Code Coverage" --results-directory:./TestResults

- name: Generate Coverage Report
  run: |
    dotnet tool install --global dotnet-reportgenerator-globaltool
    reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"TestResults/CoverageReport" -reporttypes:"Html;Cobertura"
```

## Prerequisites

### First-Time Setup
The test scripts will automatically install required tools, but you can manually install:

```powershell
# Install ReportGenerator (for coverage reports)
dotnet tool install --global dotnet-reportgenerator-globaltool

# Update if already installed
dotnet tool update --global dotnet-reportgenerator-globaltool
```

## Test Results Interpretation

### Coverage Metrics Explained

**Line Coverage**: Percentage of code lines executed during tests
- Target: 80%+ for production code
- Current: See `Summary.txt` after running tests

**Branch Coverage**: Percentage of decision branches (if/else, switch) tested
- Target: 70%+ for production code
- Current: See `Summary.txt` after running tests

**Method Coverage**: Percentage of methods executed
- Target: 90%+ for public APIs

**CRAP Score**: Change Risk Anti-Patterns
- **1-10**: Low risk, well-tested
- **11-30**: Moderate risk
- **31-60**: High risk, needs tests
- **60+**: Critical risk, requires immediate attention

### HTML Report Features

The HTML coverage report (`index.html`) provides:
- 📊 Summary dashboard with overall metrics
- 📁 Class-by-class coverage breakdown
- 📄 File-level line highlighting (green = covered, red = uncovered)
- 🔍 Method-level coverage details
- 📈 Historical trends (if run multiple times)
- 🎯 Risk indicators and hotspots

## Troubleshooting

### "reportgenerator command not found"
Run: `dotnet tool install --global dotnet-reportgenerator-globaltool`

### Coverage report is empty
Ensure tests are actually running and collecting coverage:
```powershell
dotnet test --collect:"XPlat Code Coverage" --verbosity:detailed
```

### Old coverage data showing
Clean the TestResults folder:
```powershell
Remove-Item -Recurse -Force TestResults
```

## Best Practices

### Before Committing Code
1. Run `.\test.ps1` to ensure all tests pass
2. Check coverage report for any significant drops
3. Aim to maintain or improve coverage percentage
4. Review any uncovered critical paths

### During Development
Use watch mode to run tests automatically on save:
```bash
dotnet watch test HexGrid.Tests/HexGrid.Tests.csproj
```

Or use the VS Code task: "test-watch"

### Adding New Tests
1. Create test file matching source structure
2. Follow naming convention: `{ClassName}Tests.cs`
3. Run with coverage to verify new code is tested
4. Check HTML report to ensure new methods are covered

## Integration with IDE

### Visual Studio
1. Open Test Explorer (Test > Test Explorer)
2. Tests will auto-discover from HexGrid.Tests project
3. Run with coverage: Test > Analyze Code Coverage

### Visual Studio Code
1. Install "C# Dev Kit" extension
2. Use built-in Test Explorer
3. Run tasks from Command Palette
4. CodeLens shows test/coverage status inline

### JetBrains Rider
1. Tests auto-discover in Unit Tests window
2. Right-click project > "Cover Unit Tests"
3. Built-in coverage visualization

## See Also

- [TEST-RESULTS-REPORT.md](../TEST-RESULTS-REPORT.md) - Latest detailed test report
- [HexGrid.Tests/README.md](../HexGrid.Tests/README.md) - Test suite documentation
- [NUnit Documentation](https://docs.nunit.org/) - Testing framework docs

---

**Note**: Coverage reports are generated locally and should not be committed to version control. Add `TestResults/` to `.gitignore`.
