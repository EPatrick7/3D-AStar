# 3D-AStar Demo
3D Astar Pathfinding on a Dwarf Fortress style tilemap grid stack.
*This is just a pathfinding demo, and not a full game implementation.

In Dwarf Fortress, the game takes in place in a 3D world, but the player only sees a 2D slice of that world at a time. 
In this demo, there are 10 "y-levels" available, and paths can be calculated between any two points in the 2.5D world.
The path generation is run in a coroutine to generate asynchronously, and with minimized performance impacts, even when crunching unoptimal paths.
The algorithm is color visualized, with red meaning a checked tile, yellow meaning a queued tile, and blue meaning the final path outcome.

**Controls:**
- Comma: Go down one vertical layer.
- Period: Go up one vertical layer.
- Space: Calculate Path.
- Left Click: Dig impassable hole at mouse location.
- Right Click: Place upwards-stair at mouse location.
- Left-Shift + Left Click: Move the start location for pathfinding to the mouse cursor.
- Left-Shift + Right Click: Move the end location for pathfinding to the mouse cursor.
- Right-Shift + Left Click: Place passable ground at mouse location;
- Right-Shift + Right Click: Place downwards-stair at mouse location.
