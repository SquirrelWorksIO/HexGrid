# Versioning Strategy

This document describes the semantic versioning strategy used in the HexGrid project.

## Overview

HexGrid uses **Semantic Versioning 2.0.0** (SemVer) automated by [GitVersion](https://gitversion.net/) with **[Conventional Commits](https://www.conventionalcommits.org/)**.

Version numbers follow the format: `MAJOR.MINOR.PATCH[-PRERELEASE][+BUILDMETADATA]`

Example versions:

- `1.0.0` - Production release
- `1.1.0-alpha.5` - Development pre-release
- `1.2.0-beta.2` - Release candidate
- `2.0.0-rc.1` - Release candidate for major version

## Conventional Commits

We use Conventional Commits to automatically determine version increments. This provides a standardized commit message format that both humans and tools can understand.

### Commit Message Format

```text
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Types and Version Impact

### Types and Version Impact

#### MAJOR Version (X.0.0) - Breaking Changes

Triggered by:

- Adding `!` after the type: `feat!:`, `fix!:`, etc.
- Adding `BREAKING CHANGE:` in the commit footer

**Examples:**

```bash
git commit -m "feat!: remove deprecated coordinate conversion methods"

git commit -m "fix!: change layout API signature to return Result<T>"

git commit -m "feat: redesign coordinate system

BREAKING CHANGE: AxialHexCoordinate constructor now requires explicit parameters.
Replace AxialHexCoordinate(q, r) with new AxialHexCoordinate(q, r)."
```

**When to use:**

- Removing public methods or properties
- Changing method signatures
- Changing behavior in a non-backward-compatible way
- Renaming public APIs

#### MINOR Version (0.X.0) - New Features

Triggered by: `feat:` or `feat(scope):`

**Examples:**

```bash
git commit -m "feat: add hexagonal pathfinding algorithm"
git commit -m "feat(coordinates): add support for triangular grids"
git commit -m "feat(grid): implement ring generation"
```

**When to use:**

- Adding new public methods or properties
- Adding new coordinate system support
- Adding new grid operations
- Introducing new features

#### PATCH Version (0.0.X) - Bug Fixes

Triggered by: `fix:`, `perf:`, or `revert:`

**Examples:**

```bash
git commit -m "fix: correct distance calculation for negative coordinates"
git commit -m "fix(layout): resolve pixel conversion rounding errors"
git commit -m "perf: optimize neighbor lookup algorithm"
git commit -m "revert: revert breaking API change"
```

**When to use:**

- Fixing calculation errors
- Correcting edge case handling
- Performance improvements
- Reverting problematic changes

#### NO VERSION CHANGE

Triggered by: `docs:`, `test:`, `chore:`, `style:`, `refactor:`, `build:`, `ci:`

**Examples:**

```bash
git commit -m "docs: update API documentation"
git commit -m "test: add unit tests for edge cases"
git commit -m "chore: update .gitignore"
git commit -m "style: format code"
git commit -m "refactor: extract helper method"
git commit -m "ci: update GitHub Actions workflow"
```

**When to use:**

- Documentation updates
- Adding or updating tests
- Refactoring internal code
- Code style/formatting changes
- Build configuration changes
- CI/CD configuration changes

### Legacy Format (Still Supported)

You can still use the legacy `+semver:` tags:

```bash
git commit -m "Add new feature +semver: minor"
git commit -m "Fix bug +semver: patch"
git commit -m "Breaking change +semver: major"
git commit -m "Update docs +semver: none"
```

## Branch Strategy and Pre-release Labels

### Main/Master Branch
- **Version Format:** `X.Y.Z`
- **Description:** Production-ready releases
- **Example:** `1.0.0`, `2.1.3`
- **Deployment:** Automatically published to NuGet on release

### Develop Branch
- **Version Format:** `X.Y.Z-alpha.N`
- **Description:** Active development with unstable features
- **Example:** `1.1.0-alpha.5`
- **Deployment:** Published as pre-release packages

### Release Branch (`release/x.y.z`)
- **Version Format:** `X.Y.Z-beta.N`
- **Description:** Release candidates being stabilized
- **Example:** `1.2.0-beta.1`
- **Deployment:** Published as beta packages

### Feature Branch (`feature/feature-name`)
- **Version Format:** `X.Y.Z-alpha.feature-name.N`
- **Description:** Individual feature development
- **Example:** `1.1.0-alpha.hex-pathfinding.3`
- **Deployment:** Not published, CI builds only

### Hotfix Branch (`hotfix/fix-name`)
- **Version Format:** `X.Y.Z-beta.N`
- **Description:** Critical bug fixes for production
- **Example:** `1.0.1-beta.1`
- **Deployment:** Published as beta, merged to main after testing

### Pull Request
- **Version Format:** `X.Y.Z-PullRequest.N.M`
- **Description:** Changes in pull requests
- **Example:** `1.1.0-PullRequest.42.1`
- **Deployment:** Not published, CI validation only

## Workflow Examples

### Adding a New Feature

```bash
# Create feature branch from develop
git checkout develop
git pull
git checkout -b feature/triangular-grid-support

# Make changes and commit with semantic versioning tag
git add .
git commit -m "Add triangular grid coordinate system +semver: minor"

# Push and create pull request
git push origin feature/triangular-grid-support
```

Version will be: `0.2.0-alpha.triangular-grid-support.1`

### Fixing a Bug

```bash
# Create branch from develop
git checkout develop
git checkout -b fix/neighbor-calculation

# Fix the bug
git add .
git commit -m "Fix neighbor calculation for edge coordinates +semver: patch"

# Push and create pull request
git push origin fix/neighbor-calculation
```

Version will be: `0.1.1-alpha.neighbor-calculation.1`

### Creating a Release

```bash
# Create release branch from develop
git checkout develop
git checkout -b release/1.0.0

# Make any final adjustments
git commit -m "Update version metadata +semver: none"

# Merge to main and tag
git checkout main
git merge release/1.0.0
git tag v1.0.0
git push origin main --tags

# Also merge back to develop
git checkout develop
git merge release/1.0.0
git push origin develop
```

### Hotfix for Production

```bash
# Create hotfix from main
git checkout main
git checkout -b hotfix/critical-bug-fix

# Fix the issue
git commit -m "Fix critical calculation error +semver: patch"

# Merge to main and tag
git checkout main
git merge hotfix/critical-bug-fix
git tag v1.0.1
git push origin main --tags

# Also merge to develop
git checkout develop
git merge hotfix/critical-bug-fix
git push origin develop
```

## Local Development

### Check Current Version

```bash
# Display full version information
dotnet dotnet-gitversion

# Display specific version component
dotnet dotnet-gitversion /showvariable SemVer
dotnet dotnet-gitversion /showvariable NuGetVersion
dotnet dotnet-gitversion /showvariable AssemblySemVer
```

### Build with Version

```bash
# Build with GitVersion-derived version
dotnet build /p:Version=$(dotnet dotnet-gitversion /showvariable SemVer)

# Create NuGet package with version
dotnet pack /p:PackageVersion=$(dotnet dotnet-gitversion /showvariable NuGetVersion)
```

### Without GitVersion (Fallback)

If GitVersion is not available, the build uses fallback version: `0.1.0-local`

## CI/CD Integration

### GitHub Actions

The CI/CD pipeline automatically:
1. Installs GitVersion
2. Calculates version based on branch and commits
3. Builds with calculated version
4. Creates NuGet packages
5. Publishes on releases

### Version in Build Output

The build process injects the following version properties:
- `Version` - Package version
- `AssemblyVersion` - Assembly version (binary compatibility)
- `FileVersion` - File version (displayed in file properties)
- `InformationalVersion` - Full version with metadata

## Configuration

GitVersion configuration is stored in `GitVersion.yml` at the repository root.

### Key Settings

```yaml
mode: ContinuousDeployment
tag-prefix: '[vV]'
commit-message-incrementing: Enabled
```

- **mode**: `ContinuousDeployment` increments version on every commit
- **tag-prefix**: Allows tags like `v1.0.0` or `V1.0.0`
- **commit-message-incrementing**: Uses `+semver:` tags to control versioning

## Best Practices

### DO:
✅ Use semantic versioning tags in commit messages  
✅ Tag production releases with `vX.Y.Z`  
✅ Keep breaking changes for major versions  
✅ Document breaking changes in release notes  
✅ Test pre-release packages before promoting to stable  

### DON'T:
❌ Don't manually edit version numbers in project files  
❌ Don't skip versions  
❌ Don't use inconsistent version formats  
❌ Don't tag commits on non-release branches  
❌ Don't publish alpha/beta packages as stable  

## Version History

Maintain a CHANGELOG.md file documenting all notable changes for each version:

```markdown
# Changelog

## [1.1.0] - 2025-01-15
### Added
- Hexagonal pathfinding algorithm
- Support for custom distance metrics

### Fixed
- Neighbor calculation for edge coordinates

## [1.0.0] - 2025-01-01
### Added
- Initial release
- Axial, Cube, Offset coordinate systems
- Grid layout with pointy-top and flat-top orientations
```

## Resources

- [Semantic Versioning 2.0.0](https://semver.org/)
- [GitVersion Documentation](https://gitversion.net/docs/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Keep a Changelog](https://keepachangelog.com/)

## Support

For questions about versioning:
- Check the [GitVersion documentation](https://gitversion.net/docs/)
- Open an issue on GitHub
- Contact the maintainers

---

Last Updated: December 31, 2025
