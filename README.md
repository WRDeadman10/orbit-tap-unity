# Orbit Tap

Minimal Unity 6 mobile hypercasual prototype built with primitive visuals only.

## Project Overview

Orbit Tap is a 2D arcade prototype where the player controls a dot orbiting a center point and survives incoming obstacles with simple tap input. This repository is being built in small playable steps, with each step committed independently.

## Current Status

Step 8 is complete:
- Orbit Tap is a fully playable hypercasual prototype
- The player orbits the center automatically and reacts to tap input
- Obstacles spawn from a small runtime pool and move inward
- Collisions trigger game over, score is tracked over time, and the run can be restarted
- The player now has a trail, color cycling, and smoother orbit radius transitions

Tap input is now wired for the prototype baseline and defaults to switching orbit direction.
Obstacles now spawn from a small runtime pool and move inward toward the center.
Collisions now stop the prototype immediately when the player touches an obstacle.
The prototype now has a central game manager that tracks score over time and restarts the scene after game over.
The HUD now shows live score and a restart prompt after the run ends.

## Unity Version

- `6000.3.10f1`

## Project Architecture

Current architecture is intentionally small and will grow only as gameplay systems are added.

- Scene-first setup: core runtime references start in the main scene
- Single-responsibility scripts: each script owns one piece of behaviour or presentation
- Minimal dependencies: no external assets, plugins, or framework layers beyond the Unity project template

## Folder Structure

```text
Assets/
  Prefabs/
  Scenes/
  Scripts/
  Settings/
```

## Scene Setup

`Assets/Scenes/OrbitTap.unity`

- `Main Camera`
  - Orthographic camera configured for a portrait-friendly mobile frame
- `CenterPoint`
  - Empty transform at world origin
  - Future orbit anchor for gameplay systems
- `Player`
  - Positioned on the initial orbit radius
  - Holds the visual component that renders a procedural circle

## Script Responsibilities

`CircleVisual`
- Generates a simple circular sprite at runtime/editor time
- Keeps the player visual asset-free
- Exposes only the tuning values needed for diameter, color, and texture resolution

`OrbitController`
- Moves the player around the center point using sine and cosine
- Stores orbit radius, angular speed, start angle, and direction
- Exposes small methods that later steps can call for tap actions

`TapInput`
- Reads mouse and touch taps with a single lightweight input path
- Supports switching orbit direction or cycling to the next orbit radius
- Keeps tap behavior configurable from the inspector

`BoxVisual`
- Renders pooled obstacle rectangles with primitive sprite data
- Keeps obstacle appearance configurable without art assets

`ObstacleController`
- Moves a spawned obstacle inward over time
- Applies per-obstacle size and rotation values
- Returns obstacles back to the pool when they reach the center

`ObstacleSpawner`
- Spawns inward-moving obstacles around the orbit
- Reuses obstacle instances through a basic queue-based pool
- Exposes spawn timing, speed, size, and rotation tuning in the inspector

`PlayerCollision`
- Detects player contact with pooled obstacle objects
- Reports player hits into the game state flow

`GameManager`
- Owns play/game-over state for the prototype
- Tracks score as survived time
- Reloads the active scene on restart input
- Publishes score and state changes for the UI layer

`GameHud`
- Builds a minimal runtime canvas with score and message text
- Subscribes to `GameManager` score and state events
- Displays the restart prompt after game over

`PlayerPolish`
- Adds a runtime trail renderer to the player
- Cycles player color over time and applies it to the trail
- Supports the final visual polish pass without extra assets

## Development Notes

- Use serialized fields for gameplay tuning
- Keep scripts small and readable
- Prefer scene wiring over heavy architecture until the prototype needs more structure
- Update this README and `CHANGELOG.md` on every implementation step
