Start scene file: StarterScene

## Game objective

Currently, there are 2 main scenes for players. 
In MainScene, players can walk around on a procedurally generated terrain filled with nature.
In PlanetScene, player can move around a procedurally generated planet, and defeat a total of 20 devils to win.
In the future, the LevelSelectionScene would contain miniaturized planets for players to choose to conquer.

## Controls

Move with `WASD`. Move mouse cursor to change move direction. Hold down `shift` to run. Press `space` to jump. Press `Q` to do a rapid punch + upper cut + dive roll combo. Press `ESC` to show menu.

In the editor, we can use `Environment` to set lighting to `Day`, `Night`, or `Dusk` dynamically. In the future, we will make unique planets with this.

## Packages used

### Animation clips

- RPG Character Mecanim Animation Pack FREE (we only used animation clips from this)

### Models

- BodyGuards (player character model)
- LowPolyTreePack (low poly tree prefabs)
- Devil (FloatDevil model)
- Medfan_BarrelsAndBoxes

### Shaders & materials

- QUAS-Lite (water shader)
- Skybox Cubemap Extended (skybox material & shader)
- Nicrom (wind shader that makes tree models move as if under effects of wind)

### Utility

- NavMeshComponents (from Unity, gives more fine tuned high level API controls over navmesh generation. Used as part of planetary navmesh generation process.)
- SimplexNoise (for generating a continuous 3D noise for planet terrain)

## Tech Requirements

1. Yes
2. Yes, player moves in a procedurally generated planet, with custom gravity simulation.
3. Yes, game gives fine control over player's character.
4. Yes, (PlanetScene) our game involves complex runtime navmesh generation and steering over planetary terrain, which we implemented ourselves. On the planet surface, you can find devils that track you down and attack you.
5. Yes, currently you can hit the devil till they die in PlanetScene or push bolders around in MainScene. In future we'll add more meaningful interactions.
6. Partial. Currently our game has unique and stunning procedural environment, very smooth character control, but lacks in-game UI, responsive audio (we only have punch and footstep audio for now)
7. Partial. We don't have means for player to choose how to generate the planet or spawn devils. Yet.
8. Partial. We only have start, pause, and end game condition. Currently, endgame condition triggers when you kill just 1 devil.
9. Yes, our design is very ambitious (the math for planetary interaction between objects, camera, etc. is quite involved).
10. I believe our design is very compelling and has a lot of potential.

## Known problems

See (Tech Requirements). Menu, sound, game object & logic, and in-game UI is very lacking right now. We mainly focused on the player and game environment up until now.

## Manifest

Known Problems: 
Level Selection UI Page needs further improvement
Text font, button UI needs further improvement

Manifest:

Haorong She
------------------------------------------
Work done: 
Enemy AI (player detection / steering / attacking behavior)
Enemy devil animation rigging / state machine
Enemy stats, HP, hit box and hit detection, and death events

Assets Imported: 
Devil animated character (and its animations)

Scripts worked on: 
FloatDevil.cs
Agent.cs
ComputerAgent.cs
PlayerController.cs


Meixuan Jiang
------------------------------------------
Work done: 
Player Footstep Sound (in all directions)
Player Attack Sound
Devil Getting Hit Sound
Interactive Boulders

Scripts worked on: 
PlayerSound.cs


Haoyun Xu 
------------------------------------------
Work done: 
Game Menu System Created
Game UI Initialized
Pause Game/Restart Game Enabled

Assets Imported: None, currently

Scripts worked on: 
SceneManager.cs
PauseMenuToggle.cs


Liqiong Zhao
------------------------------------------
Work done:
Game Over Scene: restart and exit buttons
Interactive Boxes

Assets Imported: Medieval barrels and boxes

Scripts worked on:
FloatDevil.cs 
SceneManager.cs


Jiachen Ren
------------------------------------------
Work done:
Overall game framework setup.
Player animation control, steering, etc.
Terrain mesh, navmesh, trees, water procedural generation.
Camera control over ground / planet.
AI agent procedural generation and steering.
Environmental lighting (day, night, dusk), skyboxes & fog, post-processing VFX.

Assets Imported: 
RPG Character Mecanim Animation Pack FREE
BodyGuards
LowPolyTreePack
Shaders & materials
QUAS-Lite
Skybox Cubemap Extended
Nicrom
NavMeshComponents 
SimplexNoise

Scripts worked on:
Everything under Assets/Scripts


