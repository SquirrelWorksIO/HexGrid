# Developer Setup Guide

Quick setup guide for HexGrid contributors.

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or later
- [Git](https://git-scm.com/)
- Your favorite C# IDE (Visual Studio, VS Code, Rider)

## Initial Setup

### 1. Clone the Repository

```bash
git clone https://github.com/SquirrelWorksIO/HexGrid.git
cd HexGrid
```

### 2. Restore .NET Tools

```bash
dotnet tool restore
```

This installs GitVersion and other local tools defined in `.config/dotnet-tools.json`.

### 3. Configure Git Commit Template (Optional but Recommended)

Set up the commit message template to guide you in writing Conventional Commits:

```bash
git config --local commit.template .gitmessage
```

Now when you run `git commit` (without `-m`), you'll see a helpful template!

### 4. Verify GitVersion Works

```bash
dotnet dotnet-gitversion
```

You should see JSON output with version information.

### 5. Build the Solution

```bash
dotnet restore
dotnet build
```

### 6. Run Tests

```bash
dotnet test
```

Or run tests with coverage:

```bash
.\test.ps1
```

## Development Workflow

### 1. Create a Feature Branch

```bash
git checkout develop
git pull
git checkout -b feature/my-awesome-feature
```

### 2. Make Changes and Commit

Use Conventional Commits format:

```bash
# Make changes
git add .
git commit
# Your editor opens with the template - follow the guidelines!

# Or use -m for quick commits
git commit -m "feat: add awesome new feature"
```

### 3. Check Your Version

See what version your commits will generate:

```bash
dotnet dotnet-gitversion /showvariable SemVer
```

### 4. Push and Create PR

```bash
git push origin feature/my-awesome-feature
```

Then create a Pull Request on GitHub targeting the `develop` branch.

## Useful Commands

### GitVersion

```bash
# Full version info
dotnet dotnet-gitversion

# Specific version component
dotnet dotnet-gitversion /showvariable SemVer
dotnet dotnet-gitversion /showvariable NuGetVersion
dotnet dotnet-gitversion /showvariable Major
dotnet dotnet-gitversion /showvariable Minor
dotnet dotnet-gitversion /showvariable Patch
```

### Building

```bash
# Debug build
dotnet build

# Release build
dotnet build --configuration Release

# Build with version
dotnet build /p:Version=$(dotnet dotnet-gitversion /showvariable SemVer)

# Clean build
dotnet clean
dotnet build
```

### Testing

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run tests with coverage (PowerShell)
.\test.ps1

# Watch mode (re-run on file changes)
dotnet watch test --project HexGrid.Tests/HexGrid.Tests.csproj
```

### Code Formatting

```bash
# Format code
dotnet format

# Check formatting without making changes
dotnet format --verify-no-changes
```

## IDE-Specific Setup

### Visual Studio 2022

1. Open `HexGrid.sln`
2. Install recommended extensions (if prompted)
3. Set startup project (if needed)
4. Use Test Explorer for running tests

### Visual Studio Code

1. Open the HexGrid folder
2. Install recommended extensions:
   - C# Dev Kit
   - .NET Extension Pack
   - GitLens (optional but helpful)
3. Press F5 to build and run tests

### JetBrains Rider

1. Open `HexGrid.sln`
2. Configure test runner (should auto-detect NUnit)
3. Enable code style settings (if prompted)

## Troubleshooting

### GitVersion Not Found

```bash
dotnet tool restore
```

### Build Errors

```bash
dotnet clean
dotnet restore
dotnet build
```

### Test Failures

Check that you're on the correct branch:

```bash
git status
git checkout develop
git pull
```

### Version Shows 0.1.0-local

This is normal for local development without GitVersion. The CI/CD pipeline will calculate the correct version.

## Resources

- [CONTRIBUTING.md](CONTRIBUTING.md) - Full contribution guidelines
- [COMMIT_CONVENTION.md](COMMIT_CONVENTION.md) - Quick commit reference
- [VERSIONING.md](VERSIONING.md) - Versioning strategy
- [Conventional Commits](https://www.conventionalcommits.org/)
- [GitVersion Docs](https://gitversion.net/docs/)

## Need Help?

- Check [GitHub Issues](https://github.com/SquirrelWorksIO/HexGrid/issues)
- Start a [Discussion](https://github.com/SquirrelWorksIO/HexGrid/discussions)
- Review the [documentation](../README.md)

---

Happy coding! 🚀
