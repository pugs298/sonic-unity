# Momentum Frontier

A playable, original high-speed 3D platformer prototype for Unity. It focuses on momentum, open-zone traversal, collectible routes, boost lanes, springs, checkpoints, and a homing dash. It uses simple procedural geometry so the project runs without external assets and can be extended with original art and animation.

## Open and play
1. Install Unity **2022.3 LTS** or newer with the WebGL Build Support module.
2. Clone this repository and open its root folder in Unity Hub.
3. Open any scene (or create an empty scene) and press **Play**. The runtime bootstrap generates the world, player, camera, and HUD automatically.

## Controls
- **WASD / arrow keys** — move relative to the camera
- **Space** — jump
- **Left/Right Shift** — boost (boost meter refills when not boosting; rings give a small refill)
- **E** — homing dash toward a nearby floating target
- **Hold right mouse button and move mouse** — orbit camera

## Prototype features
- Acceleration, momentum retention, ground friction, air steering, jump, and boost
- Large open meadow with raised isles, ramps, landmarks, and multiple traversal routes
- Ring trails and collectible bonus routes
- Boost pads, spring launchers, homing targets, checkpoints, and a finish beacon
- HUD for rings, speed, boost energy, objective messages, and zone-clear state
- Procedural geometry; no imported assets or runtime network access required

## Build for the browser (WebGL)
1. In Unity, choose **File > Build Settings** (or **Build Profiles** in newer versions).
2. Select **WebGL** and choose **Switch Platform**.
3. Add the gameplay scene to **Scenes In Build**. For this prototype, an empty scene is sufficient because the bootstrap creates the game at runtime.
4. In **Player Settings**, use **WebGL 2.0**, set **Compression Format** to **Disabled** for simple local hosting (or configure your web server to serve the chosen compression format), and set an appropriate memory size for your browser targets.
5. Choose **Build** and select an output folder. Unity creates an `index.html` and build data; host these files from a web server rather than opening the HTML directly from disk.

## Notes
This is a prototype, not a finished commercial game. The character and world are original placeholders; the project borrows only broad high-speed platformer conventions. Add original models, animation, audio, authored levels, and playtesting before release. Browser builds require Unity's WebGL support module and a static host configured for Unity's output.
