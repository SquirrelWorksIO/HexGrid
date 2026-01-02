# Script to reset Git history and start fresh
# WARNING: This will delete all commit history, branches, and tags

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "Git History Reset Script" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "WARNING: This will:" -ForegroundColor Yellow
Write-Host "  - Delete all commit history" -ForegroundColor Yellow
Write-Host "  - Remove all branches except main and develop" -ForegroundColor Yellow
Write-Host "  - Delete all tags" -ForegroundColor Yellow
Write-Host "  - Force push to remote (requires --force)" -ForegroundColor Yellow
Write-Host ""

$confirm = Read-Host "Type 'YES' to continue or anything else to cancel"
if ($confirm -ne "YES") {
    Write-Host "Cancelled." -ForegroundColor Red
    exit
}

Write-Host ""
Write-Host "Step 1: Removing .git directory..." -ForegroundColor Green
Remove-Item -Recurse -Force .git -ErrorAction SilentlyContinue

Write-Host "Step 2: Initializing new git repository..." -ForegroundColor Green
git init
git branch -M main

Write-Host "Step 3: Adding all files..." -ForegroundColor Green
git add .

Write-Host "Step 4: Creating initial commit..." -ForegroundColor Green
git commit -m "chore: initial commit

- Add HexGrid.Lib with coordinate systems and grid generation
- Add HexGrid.Tests with comprehensive test coverage
- Add HexGrid.Demo Blazor WASM application
- Configure GitVersion for semantic versioning with GitFlow
- Add comprehensive documentation"

Write-Host "Step 5: Tagging initial version..." -ForegroundColor Green
git tag v0.1.0

Write-Host "Step 6: Adding remote origin..." -ForegroundColor Green
git remote add origin git@github.com:SquirrelWorksIO/HexGrid.git

Write-Host ""
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "Git history reset complete!" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Force push main branch:" -ForegroundColor White
Write-Host "   git push -u origin main --force" -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Create and push develop branch:" -ForegroundColor White
Write-Host "   git checkout -b develop" -ForegroundColor Cyan
Write-Host "   git push -u origin develop --force" -ForegroundColor Cyan
Write-Host ""
Write-Host "3. Set develop as default branch in GitHub settings" -ForegroundColor White
Write-Host ""
Write-Host "4. Team members should re-clone the repository" -ForegroundColor White
Write-Host ""
