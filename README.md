# Momentum Frontier

A lightweight Unity 3D platformer prototype inspired by the speed and momentum of classic 3D platformers, with a broad exploration feel like an open-zone adventure. This project is intentionally built to be easy to open in Unity and export to WebGL.

## Included
- Fast 3D movement with acceleration and momentum
- Grounded jumping and boost control
- Ring pickups and win trigger
- Open-zone terrain generation
- Camera follow tuned for speed
- WebGL-friendly code using `UnityEngine` APIs only

## Unity setup
1. Install Unity 2022.3 LTS or newer.
2. Open the repository as a Unity project.
3. Create an empty scene named `Main` if one is not present.
4. Press Play. The bootstrap script will generate the player, world, camera, rings, and goal automatically.

## WebGL build
1. In Unity, open `File > Build Settings`.
2. Select `WebGL` as the target platform.
3. Click `Switch Platform`.
4. Click `Build`.
5. Serve the build locally with a static server if needed.

## Controls
- `WASD` or arrow keys: move
- `Space`: jump
- `Left Shift`: boost
- `Escape`: unlock cursor if using mouse look (not required in this prototype)

## Project structure
- `Assets/Scripts/` — game logic and world bootstrap
- `Packages/manifest.json` — Unity package setup
- `ProjectSettings/` — default Unity project settings

## Notes
This is an original game prototype inspired by fast platformers and open-zone exploration. It is intentionally lightweight and designed to be easy to extend with models, animations, sound, and more level content.
