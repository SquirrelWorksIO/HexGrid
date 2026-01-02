# GitVersion "No base versions determined" Fix

## Problem

GitVersion 6.x has a bug where it reports "No base versions determined on the current branch" even when:
- `next-version` is configured
- Tags exist on the branch
- Branch relationships are properly established

Error message:
```
ERROR [26-01-01 23:43:39:49] An unexpected error occurred:
GitVersion.GitVersionException: No base versions determined on the current branch.
INFO [26-01-01 23:43:39:48] An orphaned branch 'main' has been detected and will be skipped=True.
```

## Root Cause

GitVersion 6.x performs orphaned branch detection that **ignores** the `next-version` fallback. This is a known issue when:
- Starting with a fresh repository
- Main branch has no "source" relationship with other branches
- The orphaned branch check runs before the `next-version` fallback is applied

## Solution Options

### Option 1: Downgrade to GitVersion 5.x (RECOMMENDED)

GitVersion 5.12.0 does NOT have the orphaned branch bug and respects `next-version` properly.

```powershell
# Uninstall GitVersion 6.x
dotnet tool uninstall --global GitVersion.Tool

# Install GitVersion 5.12.0
dotnet tool install --global GitVersion.Tool --version 5.12.0
```

Then run the reset script:
```powershell
.\reset-repo.ps1
```

### Option 2: Use workflow: GitFlow/v1 Mode

Instead of `mode: ContinuousDeployment`, use the GitFlow workflow mode which has better branch relationship handling:

```yaml
workflow: GitFlow/v1
next-version: 1.0.0
tag-prefix: '[vV]'
commit-message-incrementing: Enabled
```

### Option 3: Manual Workaround with Git History

Create a proper branch relationship by making develop the parent of main:

```powershell
# Start with develop
git checkout --orphan develop
git add -A
git commit -m "chore: initial commit"

# Add GitVersion config
# ... create GitVersion.yml ...
git add GitVersion.yml
git commit -m "chore: configure GitVersion"

# Tag this commit
git tag -a v1.0.0 -m "chore: initial release"

# Create main FROM develop
git checkout -b main

# Push both
git push origin develop
git push origin main
git push origin v1.0.0
```

## Testing Locally

After applying any solution, test GitVersion:

```powershell
# On main branch
git checkout main
dotnet-gitversion /showvariable FullSemVer
# Should output: 1.0.0

# On develop branch
git checkout develop
dotnet-gitversion /showvariable FullSemVer
# Should output: 1.1.0-alpha.X
```

## CI/CD Configuration

Ensure your GitHub Actions workflow uses the correct GitVersion version:

```yaml
- name: Install GitVersion
  uses: gittools/actions/gitversion/setup@v0
  with:
    versionSpec: '5.12.0'  # <-- Use 5.x, not 6.x
```

## References

- [GitVersion Issue #3319](https://github.com/GitTools/GitVersion/issues/3319)
- [GitVersion Issue #3429](https://github.com/GitTools/GitVersion/issues/3429)
- [GitVersion Configuration Reference](https://gitversion.net/docs/reference/configuration)

## Recommended Action

**Use Option 1**: Downgrade to GitVersion 5.12.0, which is stable and doesn't have the orphaned branch detection bug.

```powershell
dotnet tool uninstall --global GitVersion.Tool
dotnet tool install --global GitVersion.Tool --version 5.12.0
.\reset-repo.ps1
```

This will give you:
- ✅ Proper version calculation on all branches
- ✅ `next-version` fallback works correctly
- ✅ No "orphaned branch" false positives
- ✅ Commit message incrementing works as expected
