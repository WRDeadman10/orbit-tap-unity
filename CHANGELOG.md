# Changelog

All notable project changes are tracked here step by step.

## Step 1 - Base Project Setup

Date: 2026-03-17

### Added
- Clean Orbit Tap base scene scaffold
- `CircleVisual` helper script for asset-free player rendering
- Root `CHANGELOG.md`

### Changed
- Renamed the main scene to `OrbitTap`
- Updated build settings to point at the new main scene
- Replaced template scene content with a minimal 2D prototype setup
- Expanded `README.md` with architecture, folder structure, and script responsibilities

### Removed
- Unity tutorial/sample assets that were not needed for the prototype

## Step 2 - Orbit System

Date: 2026-03-17

### Added
- `OrbitController` for continuous circular motion around `CenterPoint`

### Changed
- Wired the player scene object to orbit automatically using configurable radius, speed, and direction values
