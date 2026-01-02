# Breaking Change Version Bump Fix

## Problem

When making a PR with a breaking change (e.g., commit prefixed with `refactor!:`), the GitHub CI workflow did not advance the major version as expected.

**Example:**
- Version prior to PR: `1.0.0-57`
- Breaking change commit: `refactor!: rename HexGrid to HexCoordinateGrid and improve rectangular grid generation`
- Expected version: `2.0.0-*`
- Actual version: `1.0.0-58` ❌

## Root Cause

The issue was in the `GitVersion.yml` configuration for the `main` branch:

```yaml
branches:
  main:
    mode: ContinuousDelivery
    increment: Patch  # ❌ This was the problem!
    is-release-branch: true
```

**Why this caused the issue:**

When the `increment` is set to a specific value like `Patch`, `Minor`, or `Major`, it **overrides** the commit message patterns for version bumping. Even though the commit had `refactor!:` with the breaking change marker (`!`), GitVersion would only increment the patch version because of the branch configuration.

## Solution

Changed the `increment` setting to `Inherit` for the `main` branch:

```yaml
branches:
  main:
    mode: ContinuousDelivery
    increment: Inherit  # ✅ Now respects commit message patterns
    is-release-branch: true
```

**How this fixes it:**

With `increment: Inherit`, GitVersion will:
1. Look at the commit messages to determine the version bump type
2. Detect breaking change markers (`!` after type or `BREAKING CHANGE:` in footer)
3. Automatically bump the major version when a breaking change is detected
4. Respect the Conventional Commits patterns defined in the configuration

## Verification

After the fix, GitVersion correctly calculates versions based on commit messages:

- `fix:` commits → Patch bump (0.0.X)
- `feat:` commits → Minor bump (0.X.0)
- `refactor!:`, `feat!:`, or commits with `BREAKING CHANGE:` → Major bump (X.0.0)

## Files Changed

1. **GitVersion.yml** - Updated `main` branch configuration to use `increment: Inherit`
2. **GitVersion.yml** - Added `fix/*` branch configuration to prevent "No base versions determined" error
3. **docs/VERSIONING.md** - Added documentation explaining the critical importance of this setting

## Testing

To test the fix locally:

```powershell
# Check version calculation
dotnet dotnet-gitversion /showvariable SemVer

# Full version details
dotnet dotnet-gitversion
```

## Commit Convention Examples

### Major Version Bump (Breaking Change)

```bash
git commit -m "refactor!: rename HexGrid to HexCoordinateGrid"
# or
git commit -m "feat: redesign API

BREAKING CHANGE: Method signatures changed for coordinate conversion."
```

### Minor Version Bump (New Feature)

```bash
git commit -m "feat: add hexagonal pathfinding algorithm"
```

### Patch Version Bump (Bug Fix)

```bash
git commit -m "fix: correct distance calculation for negative coordinates"
```

## References

- [GitVersion Branch Configuration](https://gitversion.net/docs/reference/configuration#branches)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Semantic Versioning](https://semver.org/)

---

**Fixed:** January 1, 2026
**Issue:** Breaking changes on main branch did not trigger major version bumps
**Resolution:** Changed `main` branch `increment` from `Patch` to `Inherit` in GitVersion.yml
