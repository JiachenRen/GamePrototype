## Project: Devil Exterminator

**Team Name**: Yahaha
**Team Members**: Jiachen Ren, Liqiong Zhao, Haorong She, Meixuan Jiang, Haoyun Xu
**Game Name**: Devil Exterminator
**Start scene**: MainMenuScene

## How To Play

**Game Objective:**
Use MainMenuScene to load the game (please do not directly load PlanetScene). After clicking start, the player can move round a procedurally generated planet. The objective is to beat down 5 float devils and 1 devil boss to take revenge. The player wins if all enemies are killed. The player loses if the health reaches zero. The objective can be tracked using the onscreen UI. The enemies can be found by looking at the minimap (red spheres / arrows indicate enemies). Enemies will have random loot drops, picking up the green bottle gives the player healing points.

**Movement:**
Move the character with `WASD`. Move the mouse to control camera direction.
Hold `shift` to run. Press `space` to jump. 

**Combat:**
Press `mouse1` to do a basic punch.
Press `Q` to do a rapid punch + upper cut + dive roll combo. 

**Misc:**
Press `1` and `2` to switch between characters of different movement models.
Press `Y` to toggle minimap rotation. (follow player direction / north up)
Press `ESC` to show the pause menu. Use the graphics options to change the environment lighting to `Day`, `Night`, or `Dusk`.

## Technology requirements

1) Game must be implemented in Unity.
*Achieved*

2) Game must consist a 3D world.
*Achieved*

3) Precursor to Fun Gameplay
*Achieved. The goal to beat the boss and the sub-goal to beat the enemies are very clear (On screen UI), and there are many interesting choices for the player. The player can explore around the planet, and the player also can choose how he/she wants to attack, which also causes consequences in terms of game strategy and enemies attack. Players can also choose to change the time-of-day / environment of the planet through the options page. When the enemy is killed, the player might be rewarded with a potion to help them heal. *

4) Game must utilize a character controlled by the player with engaging animations that react to the player's input.
*Achieved, there are 2 controllable characters with different styles of movement model. Both characters are animated using the Mecanim state machine with manual optimized blending. Character animation and movement are both responsive. Also, we write our own controllers for the characters which can be found in Assets/Scripts/Player. We also have auditory feedback on character state and actions, and the scripts can be found under Assets/Scrtips/Player/PlayerControllers.cs and Assets/Scrtips/Player/Characters/BaseCharacter.cs*

5) 3D World with Physics and Spatial Simulation 
*Achieved. We have a beautiful planet environment, and we also have graphically and auditorily represented physical interactions such as foot step on dessert, foot step in the water, punch animation and sound, and more. We have an unwalkable script to restrict the character from walking there which can be found under Assets/Scrtips/Terrian/Surfaces/UnwalkableSurface.cs. We also have a variety of environmental physical interactions including interactive scripted objects (potion), destroyable objects(enemies), etc. The arisa character can move in all 8 directions by pressing w,a,s,d, wa,wd, sa, sd.*

6) Game must implement real-time steering, path planning, and state behavior AI.
*Achieved, the Float Devil and Bull Devil Boss are both real-time steering, path planning, and state behaving AI. The navigation mesh is also custom baked as the planet world is procedurally generated.*

7) Game must utilize rigid body physics simulation with interactive objects.
*Achieved, there are multiple rigid body objects such as enemy types and the player. All physics are simulated using gravity from the spherical planet with a custom gravity pull of 9.8. There are also pick-up loot drops which the player can interact with.*

8) Game must be a Game Feel game.
*Achieved, the game follows Game Feel guidelines. The game meets various Game Feel requirements such as dynamic responsive audio. (from punches, footsteps, water interactions etc) Also, the game features responsive controls with two types of player movement type.*

9) Game must provide interesting choices for the player to make.
*Achieved, there are multiple ways to take down enemies. Player can choose to use the water as cover, run out of sight to stop the chase, use roll dive to dodge, or take the enemy head on. Also there are two player models to choose from.*

10) Game must include engaging and polished starting, pauses, configuration, credits etc. via the GUI menu.
*Achieved, the game features responsive and clean interactive UI with multiple tweakable options. The players can also reload the game to restart at anypoint without quitting the application. The game provides clear starting / end game screens with follow up actions.*

11) Beware of ambitious design concepts.
*Yes, our design is very ambitious (the math for planetary interaction between objects, camera, etc. is quite involved) at first, and we even want to design multiple levels. However, during our meetings with our wonderful TA, Victor, he kindly suggested that we might not have enough time to design multiple levels, and instead we could concentrate on one level and complete that one level well. We are very glad that we took his advice and changed our game design later. Also, we have put in considerable effort to polish and make the game look visually compelling.*

12) Focus on developing a unique and compelling gameplay experience.
*I believe our design is very compelling and has a lot of potential even for future development.*

## Known problem areas

1) You might get stuck if landing from a high place:
*You can reset the player position by switching characters using `1` or `2`.*

2) Player can traverse underwater:
*This is intended behavior as the player can use it as an advantage against the enemies.*

## Manifest

**Jiachen Ren**
Tasks: Main programmer, teammate code quality assurance, overall game framework and logic, AI navigation, UI, graphics, animation controller rigging.
Assets: RPG Character Mecanim Animation Pack FREE, BodyGuards, LowPolyTreePack
Shaders & materials, QUAS-Lite, Skybox Cubemap Extended, Nicrom, ArisaNew, UnityChan, NavMeshComponents, SimplexNoise, MinimalUISounds
Scripts worked on: Everything under Assets/Scripts

**Haorong She**
Tasks: AI navigation, AI combat behavior, AI state behavior, player combat logic, stats, animation controller rigging.
Assets: Devil, Bull Devil Boss
Scripts worked on: Related scripts under Assets/Scripts

**Meixuan Jiang**
Tasks: AI, footstep sound effects, attack and combat sounds, UI elements, Interactive objects.
Assets: Footsteps, Crates and Bottles
Scripts worked on: Related scripts under Assets/Scripts

**Liqiong Zhao**
Tasks: restart and exit, gameover scene, player healing and pick-ups, sound effects, player movement.
Assets: Medieval barrels and boxes, Crates and Bottles
Scripts worked on: Related scripts under Assets/Scripts

**Haoyun Xu**
Tasks: Start and end scenes, UI elements prototype, player movement, enemy navigation prototyping, UI settings menus.
Assets: Textmesh Pro
Scripts worked on: Related scripts under Assets/Scripts