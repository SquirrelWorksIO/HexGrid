# Commit Message Convention - Quick Reference

This project uses **[Conventional Commits](https://www.conventionalcommits.org/)** for automatic semantic versioning.

## Quick Format

```text
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

## Common Examples

### ✨ New Feature (Minor Bump: 0.X.0)

```bash
git commit -m "feat: add hexagonal pathfinding"
git commit -m "feat(coordinates): add triangular grid support"
```

### 🐛 Bug Fix (Patch Bump: 0.0.X)

```bash
git commit -m "fix: correct distance calculation"
git commit -m "fix(layout): resolve pixel conversion error"
```

### 💥 Breaking Change (Major Bump: X.0.0)

```bash
# Option 1: Add ! after type
git commit -m "feat!: redesign coordinate API"

# Option 2: Use BREAKING CHANGE footer
git commit -m "feat: change constructor signature

BREAKING CHANGE: Constructor now requires explicit parameters"
```

### 📝 No Version Bump

```bash
git commit -m "docs: update README"
git commit -m "test: add unit tests"
git commit -m "chore: update dependencies"
git commit -m "style: format code"
git commit -m "refactor: simplify logic"
git commit -m "ci: update workflow"
```

## Type Reference

| Type | Description | Version Impact |
|------|-------------|----------------|
| `feat` | New feature | MINOR (0.X.0) |
| `fix` | Bug fix | PATCH (0.0.X) |
| `perf` | Performance improvement | PATCH (0.0.X) |
| `revert` | Revert previous commit | PATCH (0.0.X) |
| `!` or `BREAKING CHANGE` | Breaking change | MAJOR (X.0.0) |
| `docs` | Documentation | None |
| `test` | Tests | None |
| `chore` | Maintenance | None |
| `style` | Code style | None |
| `refactor` | Code refactoring | None |
| `build` | Build system | None |
| `ci` | CI/CD | None |

## Scope Examples

Optional parenthetical scope for more context:

- `feat(coordinates)` - Coordinate system changes
- `fix(layout)` - Grid layout fixes
- `perf(grid)` - Grid operation optimizations
- `test(api)` - API test additions
- `docs(readme)` - README updates

## Tips

✅ **DO:**
- Use imperative mood: "add" not "added"
- Keep description under 50 characters
- Start with lowercase
- No period at the end
- Reference issues: `Closes #123`

❌ **DON'T:**
- Mix multiple types in one commit
- Use past tense
- Capitalize first letter
- End with punctuation

## Full Example

```bash
git commit -m "feat(pathfinding): implement A* algorithm for hexagonal grids

Add A* pathfinding with support for:
- Custom heuristics
- Obstacle handling
- Weighted movement costs

This enables efficient path calculation for game AI and
navigation systems.

Closes #45
Related to #23"
```

## Resources

- [Conventional Commits Specification](https://www.conventionalcommits.org/)
- [Full Contributing Guide](CONTRIBUTING.md)
- [Versioning Strategy](VERSIONING.md)

---

Need help? Check the [contributing guidelines](CONTRIBUTING.md) or open an issue!
