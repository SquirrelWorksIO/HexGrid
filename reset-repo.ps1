# HexGrid Repository Reset Script
# Resets the repository with proper GitVersion configuration

param(
    [switch]$SkipBackup,
    [string]$InitialVersion = "1.0.0"
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "HexGrid Repository Reset Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Confirm with user
Write-Host "WARNING: This will completely reset your repository!" -ForegroundColor Yellow
Write-Host "Current branch will be saved as 'backup-before-reset' unless -SkipBackup is used." -ForegroundColor Yellow
Write-Host ""
$confirmation = Read-Host "Are you sure you want to continue? (yes/no)"
if ($confirmation -ne "yes") {
    Write-Host "Aborted." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Step 1: Creating backup..." -ForegroundColor Green
if (-not $SkipBackup) {
    try {
        git branch -D backup-before-reset 2>&1 | Out-Null
    } catch {}
    git branch backup-before-reset
    Write-Host "  OK Backup branch 'backup-before-reset' created" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Step 2: Resetting to clean state..." -ForegroundColor Green
# Create a new orphan branch
git checkout --orphan temp-main
git add -A
git commit -m "chore: initial commit for HexGrid solution"
Write-Host "  OK Created fresh initial commit" -ForegroundColor Gray

# Replace main
try {
    git branch -D main 2>&1 | Out-Null
} catch {}
git branch -m main
Write-Host "  OK Recreated main branch" -ForegroundColor Gray

Write-Host ""
Write-Host "Step 3: Configuring GitVersion..." -ForegroundColor Green

# Create GitVersion.yml using Out-File to avoid encoding issues
@"
mode: ContinuousDeployment
next-version: $InitialVersion
tag-prefix: '[vV]'
commit-message-incrementing: Enabled

branches:
  main:
    regex: ^main`$
    mode: ContinuousDeployment
    tag: ''
    increment: Inherit
    track-merge-target: true
    
  develop:
    regex: ^dev(elop)?(ment)?`$
    mode: ContinuousDeployment
    tag: alpha
    increment: Minor
    track-merge-target: true
    
  feature:
    regex: ^features?[/-]
    mode: ContinuousDeployment
    tag: '{BranchName}'
    increment: Inherit
    source-branches: ['develop']
    
  fix:
    regex: ^fix(es)?[/-]
    mode: ContinuousDeployment
    tag: '{BranchName}'
    increment: Inherit
    source-branches: ['develop', 'main', 'release', 'hotfix']
    
  bugfix:
    regex: ^bugfix(es)?[/-]
    mode: ContinuousDeployment
    tag: '{BranchName}'
    increment: Inherit
    source-branches: ['develop', 'main', 'release', 'hotfix']
    
  pull-request:
    regex: ^(pull|pull\-requests|pr)[/-]
    mode: ContinuousDeployment
    tag: 'pr-{BranchName}'
    increment: Inherit
    source-branches: ['develop', 'main', 'release', 'hotfix']
    
  hotfix:
    regex: ^hotfix(es)?[/-]
    mode: ContinuousDeployment
    tag: 'beta-{BranchName}'
    increment: Patch
    source-branches: ['main']
    
  release:
    regex: ^releases?[/-]
    mode: ContinuousDeployment
    tag: 'rc-{BranchName}'
    increment: None
    source-branches: ['develop', 'main', 'release', 'hotfix']

ignore:
  sha: []
"@ | Out-File -FilePath "GitVersion.yml" -Encoding UTF8 -NoNewline

git add GitVersion.yml
git commit -m "chore: configure GitVersion for automated semantic versioning"
Write-Host "  OK GitVersion.yml configured and committed" -ForegroundColor Gray

Write-Host ""
Write-Host "Step 4: Creating develop branch from main..." -ForegroundColor Green
git checkout -b develop
git commit --allow-empty -m "chore: initialize develop branch for ongoing development"
Write-Host "  OK Develop branch created from main" -ForegroundColor Gray

Write-Host ""
Write-Host "Step 5: Returning to main and tagging..." -ForegroundColor Green
git checkout main
try {
    git tag -d "v$InitialVersion" 2>&1 | Out-Null
} catch {}
git tag -a "v$InitialVersion" -m "chore: initial release v$InitialVersion"
Write-Host "  OK Tagged main as v$InitialVersion" -ForegroundColor Gray

Write-Host ""
Write-Host "Step 6: Testing GitVersion on main..." -ForegroundColor Green
$mainVersion = dotnet-gitversion /showvariable FullSemVer 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ERROR GitVersion failed on main branch!" -ForegroundColor Red
    Write-Host $mainVersion -ForegroundColor Red
    exit 1
}
Write-Host "  OK Main branch version: $mainVersion" -ForegroundColor Gray

Write-Host ""
Write-Host "Step 6: Testing GitVersion on main..." -ForegroundColor Green
$mainVersion = dotnet-gitversion /showvariable FullSemVer 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ERROR GitVersion failed on main branch!" -ForegroundColor Red
    Write-Host $mainVersion -ForegroundColor Red
    exit 1
}
Write-Host "  OK Main branch version: $mainVersion" -ForegroundColor Gray

Write-Host ""
Write-Host "Step 7: Switching to develop and testing GitVersion..." -ForegroundColor Green
git checkout develop
$developVersion = dotnet-gitversion /showvariable FullSemVer 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ERROR GitVersion failed on develop branch!" -ForegroundColor Red
    Write-Host $developVersion -ForegroundColor Red
    exit 1
}
Write-Host "  OK Develop branch version: $developVersion" -ForegroundColor Gray

Write-Host ""
Write-Host "Step 8: Reviewing git history..." -ForegroundColor Green
git log --oneline --graph --all --decorate -5

Write-Host ""
Write-Host "Step 9: Ready to push..." -ForegroundColor Green
Write-Host "  Main is at: $(git rev-parse --short main)"
Write-Host "  Develop is at: $(git rev-parse --short develop)"
Write-Host "  Tag v$InitialVersion exists on main"
Write-Host ""

$pushConfirmation = Read-Host "Push to origin? This will FORCE PUSH and overwrite remote! (yes/no)"
if ($pushConfirmation -eq "yes") {
    Write-Host ""
    Write-Host "Pushing to origin..." -ForegroundColor Green
    
    git checkout main
    git push -f origin main
    git push -f origin "v$InitialVersion"
    
    git checkout develop
    git push -f origin develop
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "OK Repository Reset Complete!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "1. Check GitHub Actions at: https://github.com/SquirrelWorksIO/HexGrid/actions" -ForegroundColor Gray
    Write-Host "2. Create feature branches from develop" -ForegroundColor Gray
    Write-Host "3. Merge develop to main to trigger RC tags" -ForegroundColor Gray
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "Push skipped. You can push manually with:" -ForegroundColor Yellow
    Write-Host "  git checkout main" -ForegroundColor Gray
    Write-Host "  git push -f origin main" -ForegroundColor Gray
    Write-Host "  git push -f origin v$InitialVersion" -ForegroundColor Gray
    Write-Host "  git checkout develop" -ForegroundColor Gray
    Write-Host "  git push -f origin develop" -ForegroundColor Gray
}
