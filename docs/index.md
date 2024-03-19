---
_disableBreadcrumb: true
---

# Welcome to **Streamer** docs!

Streamer is a simple Unity3D map streaming tool. Report all bugs or feature requests [here](https://github.com/GasimoCodes/Streamer/issues/new/choose).

Notice that the project is in early development and breaking changes are bound to appear until version 0.1. 

## Features

- Seamlessly stream Scenes based on player position.
- Automatically stream in neighbouring/denendance scenes for the current level.
- Player Enter/Leave events are exposed as C# Events for your own logic. An int is passed denoting the player "distance" from this scene. (0 - Current, 1 - Neighbour depth 1, 2 - Neighbour depth 2 etc.). This allows you to implement custom culling logic.

## Planned Features
- Object ownership (Object belongs to an area and can move to another area, changing its ownership)
- Integration with my Saving system
