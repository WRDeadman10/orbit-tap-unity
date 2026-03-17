# Orbit Tap

Minimal Unity 6 mobile hypercasual prototype built with primitive visuals only.

## Project Overview

Orbit Tap is a 2D arcade prototype where the player controls a dot orbiting a center point and survives incoming obstacles with simple tap input. This repository is being built in small playable steps, with each step committed independently.

## Current Status

Step 2 is complete:
- Unity 6 URP 2D project scaffold is in place
- The player now moves continuously around `CenterPoint`
- Main gameplay scene is set to `OrbitTap`
- Player uses a procedural circle visual with no external art assets

Tap input is now wired for the prototype baseline and defaults to switching orbit direction.
Obstacles now spawn from a small runtime pool and move inward toward the center.

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

## Development Notes

- Use serialized fields for gameplay tuning
- Keep scripts small and readable
- Prefer scene wiring over heavy architecture until the prototype needs more structure
- Update this README and `CHANGELOG.md` on every implementation step
