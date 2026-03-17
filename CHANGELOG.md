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

## Step 3 - Input System

Date: 2026-03-17

### Added
- `TapInput` for mobile-friendly tap handling

### Changed
- Connected tap input to the player so taps switch orbit direction by default
- Added an inspector-configurable alternate tap mode for radius jumps

## Step 4 - Obstacle System

Date: 2026-03-17

### Added
- `BoxVisual` for primitive obstacle rendering
- `ObstacleController` for inward-moving hazards
- `ObstacleSpawner` with a basic obstacle pool

### Changed
- Added a scene spawner that emits rotating rectangular obstacles toward the center

## Step 5 - Collision System

Date: 2026-03-17

### Added
- `PlayerCollision` for obstacle hit detection

### Changed
- Added trigger colliders to the player and pooled obstacles
- Stop the prototype immediately when a collision occurs

## Step 6 - Game Manager

Date: 2026-03-17

### Added
- `GameManager` for game state and score tracking

### Changed
- Route collisions through the game manager instead of only freezing time
- Pause orbit motion, input, and obstacle spawning when the run ends
- Support scene restart after game over

## Step 7 - UI System

Date: 2026-03-17

### Added
- `GameHud` for score and game-over text

### Changed
- Display the current score during play
- Show a restart prompt after game over

## Step 8 - Polish

Date: 2026-03-17

### Added
- `PlayerPolish` for trail and color-cycling feedback

### Changed
- Smooth orbit radius transitions instead of snapping instantly
- Add a player trail and evolving color treatment during play

## Feel Optimization

Date: 2026-03-17

### Added
- `CameraShake` for subtle death feedback

### Changed
- Smoothed orbit turning with faster, more responsive direction changes
- Added subtle idle and tap pulse scaling to the player
- Added light score bounce feedback and screen shake on death
