# Streamer
***This repository is experimental and under development. API is likely to break betweem versions until at least 0.1.0.***

Streamer is a simple level streaming tool for Unity which handles asynchronous loading/unloading of map sections based on player position. The system works well with both chunk-based approaches and linear levels.

## Setting it up
1) Add `StreamerManager` to your root scene with the player (The player needs to be marked with the `"Player"` tag and contain a valid Collider for trigger checks.
2) For each scene you wish to be handled by Streamer:
	- Create a new `StreamerArea` ScriptableObject (New/Gasimo/Streamer/StreamerArea) and configure it to point to the level it represents. 
	- If the level/chunk is connected to other scenes (or has a dependency on some far-landmark which should be visible), add these into the Dependencies field.
	![image](https://github.com/GasimoCodes/Streamer/assets/22917863/a6b918b4-7959-4b58-a07d-9b92aa4abc7a)

	- Add `StreamerPortal` into the level, setup the level bounds and assign it its `StreamerArea` we created earlier.
  ![image](https://github.com/GasimoCodes/Streamer/assets/22917863/59fa33d4-2a44-4827-860a-524b642ed8f0)

## How does it work
I created this system on my quest of getting rid of portal-based approach I used in my previous titles. These required [4 manually placed triggers](https://gasimo.itch.io/nightshift/devlog/628722/tech-behind-jen-part-2-environment) for every corridor between levels to accurately track which area *[the player is currently in, when they leave, half-enter but leave etc.]*. This new system needs just 1 collider per area and obtains player transitioning info from area bounds overlapping. The list of visited Areas is treated as a stack where top is the area the player is currently in. 

### Tracking Loss
In case the system looses track of the player  (player moves out of bounds of all the areas), the system is forced not to unload any areas until player tracking gets re-established (when player enters the bounds of any existing area). 
This is a fail-safe and a warning will be raised mentioning the last area the player was in. In case this happens, its advisable to check whether the area bounds truly encompass the entire scene or at least the areas reachable by the player. 
