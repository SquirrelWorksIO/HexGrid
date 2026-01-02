# Changelog

All notable changes to the HexGrid project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Semantic versioning using GitVersion with Conventional Commits
- Automated version management through Conventional Commit messages
- Support for standard commit types (feat, fix, docs, test, etc.)
- Automatic detection of breaking changes (! and BREAKING CHANGE)
- CI/CD integration with version calculation
- NuGet package creation with semantic versions
- Centralized build properties in Directory.Build.props
- Comprehensive versioning documentation (VERSIONING.md)
- Detailed contributing guidelines with Conventional Commits (CONTRIBUTING.md)
- Quick reference guide for commit conventions (docs/COMMIT_CONVENTION.md)
- Release workflow for automated package publishing
- GitVersion as local development tool

### Changed

- Updated GitVersion configuration to use Conventional Commits patterns
- Enhanced project files to support automatic versioning
- Improved CI/CD workflow with GitVersion integration
- Updated package metadata configuration
- README now references Conventional Commits approach

## [0.1.0] - 2025-12-31

### Added

- Initial project setup
- Hexagonal coordinate systems (Axial, Cube, Offset, Fractional)
- Grid layout support (pointy-top and flat-top orientations)
- Coordinate conversion utilities
- Distance calculations
- Neighbor finding operations
- Comprehensive unit test suite
- Code coverage reporting
- CI/CD pipeline with GitHub Actions

[Unreleased]: https://github.com/SquirrelWorksIO/HexGrid/compare/v0.1.0...HEAD
[0.1.0]: https://github.com/SquirrelWorksIO/HexGrid/releases/tag/v0.1.0
