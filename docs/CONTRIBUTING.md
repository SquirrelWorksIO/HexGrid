# Contributing to HexGrid

Thank you for your interest in contributing to HexGrid! This document provides guidelines for contributing to the project.

## Table of Contents

- [Getting Started](#getting-started)
- [Commit Message Convention](#commit-message-convention)
- [Versioning](#versioning)
- [Development Workflow](#development-workflow)
- [Pull Request Process](#pull-request-process)
- [Code Style](#code-style)
- [Testing](#testing)

## Getting Started

1. **Fork the repository** on GitHub
2. **Clone your fork** locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/HexGrid.git
   cd HexGrid
   ```
3. **Install .NET 10.0 SDK** or later
4. **Restore dependencies**:
   ```bash
   dotnet restore
   ```
5. **Build the solution**:
   ```bash
   dotnet build
   ```
6. **Run tests**:
   ```bash
   dotnet test
   ```

## Commit Message Convention

This project uses **[Conventional Commits](https://www.conventionalcommits.org/)** for automatic semantic versioning.

### Format

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Types

The type **must** be one of the following:

#### Version-Impacting Types

- **feat**: A new feature (triggers **MINOR** version bump: 0.X.0)
  ```bash
  feat: add hexagonal pathfinding algorithm
  feat(coordinates): add support for triangular grids
  ```

- **fix**: A bug fix (triggers **PATCH** version bump: 0.0.X)
  ```bash
  fix: correct distance calculation for negative coordinates
  fix(layout): resolve pixel conversion rounding errors
  ```

- **perf**: Performance improvement (triggers **PATCH** version bump: 0.0.X)
  ```bash
  perf: optimize neighbor lookup algorithm
  perf(grid): cache frequently accessed coordinates
  ```

- **revert**: Reverts a previous commit (triggers **PATCH** version bump: 0.0.X)
  ```bash
  revert: revert "feat: add triangular grid support"
  ```

#### Breaking Changes (MAJOR version bump: X.0.0)

Add `!` after the type or `BREAKING CHANGE:` in the footer:

```bash
# Option 1: Using ! after type
feat!: remove deprecated coordinate conversion methods
fix!: change layout API signature

# Option 2: Using BREAKING CHANGE in footer
feat: redesign coordinate system API

BREAKING CHANGE: The AxialHexCoordinate constructor now requires explicit parameters.
Migration guide: Replace AxialHexCoordinate(q, r) with new AxialHexCoordinate(q, r).
```

#### Non-Version-Impacting Types

- **docs**: Documentation only changes (no version bump)
  ```bash
  docs: update README with new installation instructions
  docs(api): add XML comments to public methods
  ```

- **style**: Code style changes (formatting, missing semicolons, etc.) (no version bump)
  ```bash
  style: format code with dotnet format
  style(lib): fix indentation in coordinate classes
  ```

- **refactor**: Code refactoring without changing behavior (no version bump)
  ```bash
  refactor: extract common validation logic
  refactor(tests): simplify test setup helpers
  ```

- **test**: Adding or updating tests (no version bump)
  ```bash
  test: add tests for edge cases in distance calculation
  test(layout): improve coverage for orientation tests
  ```

- **build**: Changes to build system or dependencies (no version bump)
  ```bash
  build: update NuGet packages
  build(deps): upgrade to .NET 10.0
  ```

- **ci**: Changes to CI/CD configuration (no version bump)
  ```bash
  ci: add code coverage reporting
  ci(github): update workflow to use GitVersion
  ```

- **chore**: Other changes that don't modify src or test files (no version bump)
  ```bash
  chore: update .gitignore
  chore: clean up temporary files
  ```

### Scope (Optional)

The scope provides additional context:

```bash
feat(coordinates): add cubic coordinate system
fix(layout): resolve pixel conversion issue
test(grid): add integration tests
docs(api): document public interfaces
```

Common scopes:
- `coordinates` - Coordinate system changes
- `layout` - Grid layout changes
- `grid` - Grid operations
- `api` - Public API changes
- `tests` - Test-related changes
- `ci` - CI/CD changes
- `docs` - Documentation changes

### Description

- Use imperative, present tense: "add" not "added" or "adds"
- Don't capitalize first letter
- No period (.) at the end
- Keep it concise (50 characters or less if possible)

### Body (Optional)

- Use imperative, present tense
- Include motivation for the change
- Contrast with previous behavior
- Wrap at 72 characters

### Footer (Optional)

- Reference GitHub issues: `Closes #123`, `Fixes #456`
- Document breaking changes: `BREAKING CHANGE: ...`
- Link related issues: `Related to #789`

### Examples

#### Simple Feature

```bash
git commit -m "feat: add hexagonal ring generation"
```

**Result**: Minor version bump (e.g., 0.1.0 → 0.2.0)

#### Bug Fix with Scope

```bash
git commit -m "fix(layout): correct pixel-to-hex conversion for flat-top orientation"
```

**Result**: Patch version bump (e.g., 0.2.0 → 0.2.1)

#### Feature with Body and Footer

```bash
git commit -m "feat(pathfinding): implement A* pathfinding algorithm

Add A* pathfinding for hexagonal grids with support for:
- Custom heuristics
- Obstacle handling
- Weighted paths

Closes #45"
```

**Result**: Minor version bump

#### Breaking Change

```bash
git commit -m "feat!: redesign coordinate conversion API

BREAKING CHANGE: The coordinate conversion methods now return Result<T>
instead of throwing exceptions. Update your error handling:

Before: var hex = layout.PixelToHex(point);
After: var result = layout.PixelToHex(point);
       if (result.IsSuccess) { var hex = result.Value; }

Migration guide available at docs/migration/v2.0.0.md

Closes #67"
```

**Result**: Major version bump (e.g., 1.5.2 → 2.0.0)

#### Multiple Changes

```bash
git commit -m "feat: add multiple coordinate system improvements

- Add triangular grid support
- Implement coordinate caching
- Optimize distance calculations

This provides better performance and flexibility for different grid types.

Closes #12, #34, #56"
```

#### No Version Bump

```bash
git commit -m "docs: update contribution guidelines"
git commit -m "test: add unit tests for edge cases"
git commit -m "chore: update .gitignore"
```

**Result**: No version bump

## Versioning

This project follows [Semantic Versioning](https://semver.org/) (`MAJOR.MINOR.PATCH`):

- **MAJOR**: Breaking changes (via `!` or `BREAKING CHANGE:`)
- **MINOR**: New features (via `feat:`)
- **PATCH**: Bug fixes (via `fix:`, `perf:`, `revert:`)

Version is automatically calculated by GitVersion based on:
1. Conventional commit messages
2. Git branch name
3. Git tags

### Manual Override (Legacy)

If needed, you can still use the legacy `+semver:` tags:

```bash
git commit -m "custom change +semver: minor"
git commit -m "important fix +semver: patch"
git commit -m "breaking change +semver: major"
git commit -m "no version change +semver: none"
```

## Development Workflow

### 1. Create a Feature Branch

```bash
git checkout develop
git pull origin develop
git checkout -b feature/my-awesome-feature
```

### 2. Make Changes

```bash
# Edit files
# Add tests
# Update documentation
```

### 3. Run Tests

```bash
dotnet test
```

### 4. Commit Using Conventional Commits

```bash
git add .
git commit -m "feat: add awesome new feature"
```

### 5. Push and Create Pull Request

```bash
git push origin feature/my-awesome-feature
```

Then create a PR on GitHub targeting the `develop` branch.

## Pull Request Process

1. **Update Documentation**: Ensure README and XML docs are updated
2. **Add Tests**: Include unit tests for new features or bug fixes
3. **Follow Conventional Commits**: Use proper commit message format
4. **Pass CI Checks**: Ensure all tests and builds pass
5. **Code Review**: Address reviewer feedback
6. **Squash Commits** (optional): Consider squashing before merge
7. **Merge**: Maintainer will merge after approval

### PR Title Format

Use Conventional Commits format for PR titles:

```
feat: add hexagonal pathfinding
fix: correct distance calculation bug
docs: update API documentation
```

## Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments to public APIs
- Keep methods focused and concise
- Use modern C# features (nullable reference types, pattern matching, etc.)

### Formatting

```bash
# Format code before committing
dotnet format
```

## Testing

### Writing Tests

- Write tests for all new features
- Write tests for bug fixes
- Maintain or improve code coverage
- Use descriptive test names

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
.\test.ps1

# Watch mode (re-run on changes)
dotnet watch test --project HexGrid.Tests/HexGrid.Tests.csproj
```

### Coverage Requirements

- Aim for 80%+ code coverage
- Cover edge cases and error conditions
- Test public APIs thoroughly

## Questions or Problems?

- **Issues**: [GitHub Issues](https://github.com/SquirrelWorksIO/HexGrid/issues)
- **Discussions**: [GitHub Discussions](https://github.com/SquirrelWorksIO/HexGrid/discussions)

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to HexGrid! 🎮🔷
