# GitFlow Versioning Strategy

## Overview

This document explains how semantic versioning works with our GitFlow workflow where:
1. **Feature/Fix branches** → PR into **develop**
2. **Develop branch** → PR into **main**

## Branch Configuration Summary

| Branch | Mode | Increment | Label | Purpose |
|--------|------|-----------|-------|---------|
| `main` | ContinuousDelivery | **Inherit** | none | Production releases - respects commit messages |
| `develop` | ContinuousDeployment | **Minor** | alpha | Integration branch - always bumps minor |
| `release/*` | ContinuousDelivery | None | beta | Release candidates |
| `feature/*` | ContinuousDeployment | Inherit | {BranchName} | New features - respects commit messages |
| `fix/*` | ContinuousDeployment | Inherit | {BranchName} | Bug fixes - respects commit messages |
| `hotfix/*` | ContinuousDeployment | Patch | beta | Emergency fixes for production |

## Why `develop` Uses `increment: Minor`

**Important:** The `develop` branch intentionally uses `increment: Minor` instead of `Inherit` for these reasons:

### 1. **Consistent Pre-release Versioning**
When multiple PRs merge into `develop`, each merge creates a new alpha version:
- First merge: `1.0.0-alpha.1`
- Second merge: `1.1.0-alpha.1`
- Third merge: `1.2.0-alpha.1`

This provides a predictable versioning scheme for development builds.

### 2. **`track-merge-target: true` Behavior**
The `develop` branch has `track-merge-target: true`, which means:
- When you merge `develop` → `main`, GitVersion looks at the commits that came FROM feature/fix branches
- Those commits are analyzed for breaking changes, feat, fix patterns
- The **main branch will calculate the correct version** based on those commits

### 3. **Version Calculation Flow**

#### Example: Feature Branch → Develop → Main

**Step 1: Feature Branch**
```bash
# On feature/add-triangular-grid
git commit -m "feat: add triangular grid support"
# Version: 1.1.0-feature-add-triangular-grid.1
```

**Step 2: Merge to Develop**
```bash
# PR merged to develop
# Version on develop: 1.1.0-alpha.1
# (Uses Minor increment, adds alpha label)
```

**Step 3: Merge to Main**
```bash
# PR from develop to main
# Version on main: 1.1.0
# (Inherits version based on feat: commit from feature branch)
```

#### Example: Breaking Change Flow

**Step 1: Feature Branch with Breaking Change**
```bash
# On feature/api-redesign
git commit -m "refactor!: redesign coordinate API

BREAKING CHANGE: Renamed HexGrid to HexCoordinateGrid"
# Version: 2.0.0-feature-api-redesign.1
```

**Step 2: Merge to Develop**
```bash
# PR merged to develop
# Version on develop: 2.0.0-alpha.1
# (Breaking change detected, bumps major even though develop has increment: Minor)
```

**Step 3: Merge to Main**
```bash
# PR from develop to main
# Version on main: 2.0.0
# (main has increment: Inherit, respects breaking change)
```

## Version Examples by Branch

### Main Branch (Production)
- **After fix PR merged from develop:** `1.0.1`
- **After feat PR merged from develop:** `1.1.0`
- **After breaking change from develop:** `2.0.0`

Versions on `main` are **clean semantic versions** without pre-release labels.

### Develop Branch (Integration)
- **After fix/* PR:** `1.1.0-alpha.1`
- **After feature/* PR:** `1.2.0-alpha.1`
- **After multiple merges:** `1.3.0-alpha.1`, `1.4.0-alpha.1`, etc.

Versions on `develop` have the **alpha** label for pre-release identification.

### Feature Branches
- `feature/add-pathfinding`: `1.1.0-add-pathfinding.1`
- `feature/triangular-grid`: `1.2.0-triangular-grid.5`

Versions include the **branch name** for traceability.

### Fix Branches
- `fix/distance-calculation`: `1.0.1-distance-calculation.1`
- `fix/neighbor-lookup`: `1.0.2-neighbor-lookup.3`

Versions include the **branch name** and patch increment.

## Workflow Examples

### Adding a New Feature

```bash
# 1. Create feature branch from develop
git checkout develop
git pull origin develop
git checkout -b feature/hexagonal-pathfinding

# 2. Make changes and commit with conventional commits
git commit -m "feat: add hexagonal pathfinding algorithm"
# Version: 1.1.0-hexagonal-pathfinding.1

# 3. Push and create PR to develop
git push origin feature/hexagonal-pathfinding
# Create PR: feature/hexagonal-pathfinding → develop

# 4. After PR merged, develop version becomes
# develop: 1.1.0-alpha.1 (or next minor version)

# 5. When develop is merged to main
# main: 1.1.0 (clean release version)
```

### Fixing a Bug

```bash
# 1. Create fix branch from develop
git checkout develop
git checkout -b fix/coordinate-conversion

# 2. Fix the bug
git commit -m "fix: correct coordinate conversion for negative values"
# Version: 1.0.1-coordinate-conversion.1

# 3. PR to develop
# develop version after merge: 1.1.0-alpha.1
# (Minor bump because develop always uses Minor)

# 4. When develop merged to main
# main: 1.0.1
# (Patch bump because of fix: commit)
```

### Breaking Change

```bash
# 1. Create feature branch
git checkout develop
git checkout -b feature/api-v2

# 2. Make breaking change
git commit -m "refactor!: redesign public API

BREAKING CHANGE: Method signatures changed for coordinate conversion"
# Version: 2.0.0-api-v2.1

# 3. PR to develop
# develop: 2.0.0-alpha.1
# (Major bump detected despite develop's Minor setting)

# 4. PR from develop to main
# main: 2.0.0
# (Major version bump preserved)
```

## Important Notes

### ✅ DO:

- **Use conventional commits** on feature/fix branches - they control the final version
- **Merge feature/fix branches to develop first**
- **Merge develop to main** for production releases
- **Tag releases on main** with `v{version}` format
- **Test alpha versions** from develop before merging to main

### ❌ DON'T:

- **Don't merge feature branches directly to main** - break GitFlow
- **Don't manually edit version numbers** - let GitVersion handle it
- **Don't worry about develop's Minor increment** - main will calculate correctly
- **Don't change develop to increment: Inherit** - breaks versioning predictability

## Troubleshooting

### Issue: Version not bumping on main

**Cause:** Commits on develop don't have conventional commit prefixes

**Solution:** Use proper conventional commits on feature/fix branches:
- `feat:` for new features
- `fix:` for bug fixes  
- `refactor!:` or `BREAKING CHANGE:` for breaking changes

### Issue: Develop version seems wrong

**Behavior:** This is expected! Develop always increments minor version for predictable alpha versioning.

**Result:** Main will calculate the correct version based on the original commits when develop merges.

### Issue: "No base versions determined" error

**Cause:** Branch pattern not configured in GitVersion.yml

**Solution:** Ensure all branch types (feature/*, fix/*, etc.) are configured in `GitVersion.yml`

## Configuration Reference

See `GitVersion.yml` for the complete configuration. Key settings:

```yaml
mode: ContinuousDeployment
commit-message-incrementing: Enabled

branches:
  main:
    increment: Inherit  # Respects conventional commits
  
  develop:
    increment: Minor  # Always bumps minor for alpha versions
    track-merge-target: true  # Analyzes commits from feature branches
```

## Related Documentation

- [Conventional Commits](https://www.conventionalcommits.org/)
- [GitFlow Workflow](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)
- [docs/VERSIONING.md](VERSIONING.md) - General versioning strategy
- [docs/COMMIT_CONVENTION.md](COMMIT_CONVENTION.md) - Commit message guidelines

---

**Last Updated:** January 1, 2026
