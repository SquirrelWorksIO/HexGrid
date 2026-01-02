# Automated Versioning Workflow

## Overview

The HexGrid project uses **automated semantic versioning** based on conventional commits and GitFlow workflow.

## Version Calculation Rules

### Commit Message → Version Bump

| Commit Type | Version Bump | Example |
|-------------|--------------|---------|
| `feat:` or `feature:` | **Minor** (0.X.0) | `feat: add pathfinding` → 0.2.0 |
| `fix:` or `bugfix:` | **Patch** (0.0.X) | `fix: correct calculation` → 0.1.1 |
| Any type with `!` | **Major** (X.0.0) | `refactor!: redesign API` → 2.0.0 |
| Other types | No bump | `docs: update readme` → no change |

### Priority Order (Highest to Lowest)

When multiple commits are in a PR, the **highest priority** determines the version bump:

1. **Breaking changes** (`!` after type)
2. **Features** (`feat:` or `feature:`)
3. **Fixes** (`fix:` or `bugfix:`)
4. **Others** (no bump)

## Workflow

### 1. Feature Development

```bash
# Create feature branch from develop
git checkout develop
git pull origin develop
git checkout -b feature/add-pathfinding

# Make changes and commit with conventional commits
git commit -m "feat: add hexagonal pathfinding algorithm"

# Push and create PR to develop
git push origin feature/add-pathfinding
```

**Version on feature branch:** `0.2.0-add-pathfinding.1`

### 2. Merge to Develop

- PR is reviewed and merged to `develop`
- CI runs: build, test, version calculation
- **Version on develop:** `0.2.0-alpha.1`

### 3. Merge to Main (Release Candidate)

```bash
# Create PR from develop to main
# Once approved and merged:
```

**Automatic actions:**
- CI runs: build, test, version calculation
- **Version calculated:** `0.2.0-rc`
- **Tag created:** `v0.2.0-rc`
- **GitHub Release created** (pre-release)

### 4. Promote to Stable Release

When ready to release:

1. Go to **Actions** → **Promote to Release**
2. Click **Run workflow**
3. Enter the RC version: `0.2.0-rc`
4. Click **Run workflow**

**Automatic actions:**
- Validates RC tag exists
- Creates stable tag: `v0.2.0`
- Creates stable GitHub Release

## Version Format by Branch

| Branch | Version Format | Example |
|--------|----------------|---------|
| `feature/*` | `X.Y.Z-{branch}.N` | `0.2.0-add-pathfinding.3` |
| `fix/*` | `X.Y.Z-{branch}.N` | `0.1.1-distance-calc.1` |
| `bugfix/*` | `X.Y.Z-{branch}.N` | `0.1.2-null-check.2` |
| `develop` | `X.Y.Z-alpha.N` | `0.2.0-alpha.5` |
| `main` | `X.Y.Z-rc` | `0.2.0-rc` |
| Stable | `X.Y.Z` | `0.2.0` |

## Examples

### Minor Version Bump (Feature)

```bash
# On feature/ui-improvements
git commit -m "feat: add color themes"
```
- Feature branch: `0.2.0-ui-improvements.1`
- After merge to develop: `0.2.0-alpha.1`
- After merge to main: `0.2.0-rc` → `v0.2.0-rc` tag
- After promotion: `0.2.0` → `v0.2.0` tag

### Patch Version Bump (Fix)

```bash
# On fix/calculation-error
git commit -m "fix: correct distance calculation for negative coordinates"
```
- Fix branch: `0.1.1-calculation-error.1`
- After merge to develop: `0.1.1-alpha.1`
- After merge to main: `0.1.1-rc` → `v0.1.1-rc` tag
- After promotion: `0.1.1` → `v0.1.1` tag

### Major Version Bump (Breaking Change)

```bash
# On feature/api-v2
git commit -m "refactor!: redesign coordinate API

BREAKING CHANGE: Method signatures changed for all coordinate operations"
```
- Feature branch: `2.0.0-api-v2.1`
- After merge to develop: `2.0.0-alpha.1`
- After merge to main: `2.0.0-rc` → `v2.0.0-rc` tag
- After promotion: `2.0.0` → `v2.0.0` tag

### Multiple Commits in PR (Priority)

```bash
# PR with multiple commits
git commit -m "docs: update API documentation"    # No bump
git commit -m "fix: correct null check"            # Patch
git commit -m "feat: add new grid type"            # Minor <- WINS (highest priority)
```

Result: **Minor version bump** (feature has higher priority than fix)

## CI/CD Pipelines

### CI Pipeline (`ci.yml`)

**Triggers:**
- Push to `main`, `develop`
- PRs to `main`, `develop`

**Jobs:**
1. **build-test**: Build, test, calculate version
2. **tag-rc**: (main only) Create RC tag and GitHub release

### Release Pipeline (`release.yml`)

**Trigger:** Manual (workflow_dispatch)

**Process:**
1. Validate RC tag exists
2. Extract stable version
3. Create stable tag
4. Create stable GitHub release

## Tips

### ✅ DO

- Use conventional commit format: `type: description`
- Add `!` after type for breaking changes: `refactor!:`
- Use descriptive commit messages
- Test changes before merging to develop
- Review RC before promoting to stable

### ❌ DON'T

- Don't mix breaking changes with features in same PR
- Don't manually create version tags
- Don't edit version numbers in code
- Don't merge directly to main (use PRs)
- Don't skip conventional commit format

## Troubleshooting

### Version not bumping correctly

**Check:** Commit message format
- Must start with type: `feat:`, `fix:`, etc.
- Breaking changes need `!`: `feat!:`

### CI failing on version determination

**Check:**
- Tags are fetched (`fetch-tags: true` in workflow)
- GitVersion.yml exists and is valid
- Base version tag exists (`v0.1.0`)

### RC not created after merge to main

**Check:**
- PR was merged (not force pushed)
- CI workflow completed successfully
- GitHub Actions has write permissions

## Configuration Files

- **GitVersion.yml**: Version calculation rules
- **.github/workflows/ci.yml**: CI pipeline
- **.github/workflows/release.yml**: Release promotion

## References

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Semantic Versioning](https://semver.org/)
- [GitVersion Documentation](https://gitversion.net/docs/)

---

**Last Updated:** January 1, 2026
