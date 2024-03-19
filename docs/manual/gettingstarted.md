---
_layout: landing
---

## Quick Start:

### UPM:

# [2022.2 and above](#tab/newer)

1. Import Streamer `https://github.com/GasimoCodes/Streamer.git?path=com.gasimo.streamer` into **Package Manager** 
  


---

### Setting up
1) Add `StreamerManager` to your root scene with the player (The player needs to be marked with the `"Player"` tag and contain a valid Collider for trigger checks.
2) For each scene you wish to be handled by Streamer:
	- Create a new `StreamerArea` ScriptableObject (New/Gasimo/Streamer/StreamerArea) and configure it to point to the level it represents. 
	- If the level/chunk is connected to other scenes (or has a dependency on some far-landmark which should be visible), add these into the Dependencies field.
	![image](https://github.com/GasimoCodes/Streamer/assets/22917863/a6b918b4-7959-4b58-a07d-9b92aa4abc7a)

	- Add `StreamerPortal` into the level, setup the level bounds and assign it its `StreamerArea` we created earlier.
  ![image](https://github.com/GasimoCodes/Streamer/assets/22917863/59fa33d4-2a44-4827-860a-524b642ed8f0)
