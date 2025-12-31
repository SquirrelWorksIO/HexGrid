# HexGrid

[![CI/CD Pipeline](https://github.com/SquirrelWorksIO/HexGrid/actions/workflows/ci.yml/badge.svg)](https://github.com/SquirrelWorksIO/HexGrid/actions/workflows/ci.yml)
[![Release](https://github.com/SquirrelWorksIO/HexGrid/actions/workflows/release.yml/badge.svg)](https://github.com/SquirrelWorksIO/HexGrid/actions/workflows/release.yml)
[![codecov](https://codecov.io/gh/SquirrelWorksIO/HexGrid/branch/develop/graph/badge.svg)](https://codecov.io/gh/SquirrelWorksIO/HexGrid)
[![NuGet](https://img.shields.io/nuget/v/HexGrid.Lib.svg)](https://www.nuget.org/packages/HexGrid.Lib/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A comprehensive hexagonal grid library for .NET that provides coordinate systems, layouts, and utilities for working with hexagonal grids in games and applications.

## Features

- **Multiple Coordinate Systems**: Axial, Cube, Offset, and Fractional coordinates
- **Flexible Layouts**: Support for both pointy-top and flat-top orientations
- **Coordinate Conversions**: Seamless conversion between different coordinate systems
- **Grid Operations**: Distance calculations, neighbor finding, line drawing, and more
- **Type-Safe**: Built with modern C# features and nullable reference types
- **Well-Tested**: Comprehensive unit test coverage
- **Performance**: Optimized for game development and real-time applications

## Installation

### NuGet Package Manager

```bash
dotnet add package HexGrid.Lib
```

### Package Manager Console

```powershell
Install-Package HexGrid.Lib
```

## Quick Start

```csharp
using HexGrid.Lib;
using HexGrid.Lib.Coordinates;
using HexGrid.Lib.Layout;

// Create a hexagonal grid with pointy-top orientation
var orientation = LayoutOrientation.PointyTop;
var origin = new PointD(0, 0);
var size = new PointD(30, 30); // Hex size in pixels

var layout = new GridLayout(orientation, size, origin);

// Create a hex coordinate
var hex = new AxialHexCoordinate(2, 3);

// Convert to pixel coordinates
var pixel = layout.HexToPixel(hex);

// Get neighbors
var neighbors = hex.GetNeighbors();

// Calculate distance between hexes
var distance = hex.DistanceTo(new AxialHexCoordinate(5, 1));
```

## Versioning

This project uses **Semantic Versioning** (SemVer) automated by [GitVersion](https://gitversion.net/) with **[Conventional Commits](https://www.conventionalcommits.org/)**.

### Commit Message Format

Use Conventional Commits to automatically control versioning:

```bash
# Minor version bump (new feature)
git commit -m "feat: add hexagonal pathfinding algorithm"

# Patch version bump (bug fix)
git commit -m "fix: correct distance calculation for edge cases"

# Major version bump (breaking change)
git commit -m "feat!: redesign coordinate conversion API"
# or
git commit -m "feat: redesign coordinate API

BREAKING CHANGE: Coordinate constructors now require explicit parameters."

# No version bump
git commit -m "docs: update API documentation"
git commit -m "test: add unit tests for edge cases"
```

### Conventional Commit Types

- **feat**: New feature → **MINOR** version bump (0.X.0)
- **fix**: Bug fix → **PATCH** version bump (0.0.X)
- **perf**: Performance improvement → **PATCH** version bump (0.0.X)
- **BREAKING CHANGE** or **!** after type → **MAJOR** version bump (X.0.0)
- **docs**, **test**, **chore**, **style**, **refactor** → No version bump

See [CONTRIBUTING.md](docs/CONTRIBUTING.md) for detailed guidelines.

### Branch Strategy

- **main/master**: Production-ready releases (e.g., `1.0.0`)
- **develop**: Active development with alpha pre-releases (e.g., `1.1.0-alpha.5`)
- **release/x.x.x**: Release candidates with beta pre-releases (e.g., `1.1.0-beta.1`)
- **feature/**: Feature branches (e.g., `1.1.0-alpha.my-feature.3`)
- **hotfix/**: Critical fixes (e.g., `1.0.1-beta.1`)

### Local Development

When building locally without GitVersion, the version defaults to `0.1.0-local`.

To check the version that GitVersion will generate:

```bash
# Install GitVersion as a global tool
dotnet tool install --global GitVersion.Tool

# Display version information
dotnet-gitversion

# Display specific version component
dotnet-gitversion /showvariable SemVer
```

### Creating a Release

1. **Merge to main**: Merge your develop branch to main
2. **Create a Git tag**:

   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

3. **Create GitHub Release**: The Release workflow will automatically build and publish

## Development

### Prerequisites

- .NET 10.0 SDK or later
- Git

### Building

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Run tests with coverage
.\test.ps1
```

### Project Structure

```text
HexGrid/
├── .github/
│   └── workflows/          # CI/CD workflows
├── HexGrid.Lib/            # Main library
│   ├── Coordinates/        # Coordinate systems
│   ├── Layout/            # Grid layout and positioning
│   └── Models/            # Core models and utilities
├── HexGrid.Tests/         # Unit tests
├── GitVersion.yml         # Semantic versioning configuration
├── Directory.Build.props  # Centralized build properties
└── HexGrid.sln           # Solution file
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Watch mode (re-run on file changes)
dotnet watch test --project HexGrid.Tests/HexGrid.Tests.csproj
```

### Code Coverage

Test coverage reports are automatically generated in `TestResults/CoverageReport/`.

Open the HTML report:
```bash
# Windows
start TestResults/CoverageReport/index.html

# PowerShell
Invoke-Item TestResults/CoverageReport/index.html
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes with Conventional Commits format
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Commit Message Guidelines

Use Conventional Commits format. See [docs/CONTRIBUTING.md](docs/CONTRIBUTING.md) for detailed guidelines.

**Quick examples:**

```text
feat: add hexagonal ring generation
fix: correct neighbor calculation for edge cases
docs: update API documentation
refactor: optimize distance calculations
BREAKING CHANGE: remove deprecated coordinate methods
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Hexagonal grid algorithms based on [Red Blob Games' excellent guide](https://www.redblobgames.com/grids/hexagons/)
- Built with modern .NET and C# best practices

## Support

- **Issues**: [GitHub Issues](https://github.com/SquirrelWorksIO/HexGrid/issues)
- **Discussions**: [GitHub Discussions](https://github.com/SquirrelWorksIO/HexGrid/discussions)

---

Made with ❤️ by SquirrelWorks
