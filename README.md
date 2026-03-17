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

## Development Notes

- Use serialized fields for gameplay tuning
- Keep scripts small and readable
- Prefer scene wiring over heavy architecture until the prototype needs more structure
- Update this README and `CHANGELOG.md` on every implementation step
