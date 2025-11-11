For codebase documentation, see:
https://allie-writes-code.github.io/Portfolio-Showcase-Project-1-2025-Gormoth/

====================
Changlog:
====================

===== 11/11/25 =====
Changes:
- Updated Stats to now allow them to act as a Boolean. Still use the main values but if called as a bool, a 0 will return a false and any non 0 number will return a true. Added a flag on Stat, (bool) isBoolValue that can only be set in editor.
- Updated GameResourceManager to account for bool Stats. Converts the value to a string when saving and back into a float when loading, to better show that the Stat is a Boolean in the JSON file - i.e. value will be 0 as a float but saved as 'False' in JSON. Editing the value to true or false in the JSON file loads back in as 0 or 1 respectively. The Boolean (isBoolValue) on Stat instances is used to control if GameResourceManager does this for each Stat.


===== 10/11/25 =====
Changes:
- Added GameResourceManager class. Class performs two major functions, will load all Stat instances from a specified folder and save selected values to a JSON file and the inverse, will load values from JSON and pump them into the game stats. This enables two really cool things, 1) Can be used to easily defined default / starting Stats without having to manually check the objects in editor and 2) makes the game highly moddable for end users, they can edit the JSON file and set initial starting values. I want to expand on this, set the default / base game values so they can't be edited (to allow for 'no cheating' gameplay - i.e. for achievements, should they ever be implemented) and allow users to save, export and import set 'mod' folders. Ideally, this could also expand into loading all building or world objects from JSON lists too, to open those up too.

===== 7/11/25 =====
Fixes:
- Adjusted pathfinding grid settings (see changes for details) to line up pathfinding nodes with world object nodes - position and size - and adjusted walkable check to ensure it was only putting unwalkable nodes where there were objects (as game grid is 1x1 g.u. nodes). 

Changes:
- Put Pathfinding walkable checksphere into own function, was split between the grid creation and update walkable function. Both now just call the same function to make changes easier.
- Adjusted NodeGrid generation. Position was calculated using x or y * nodeDiameter - nodeRadius, this was causing the grid to be 'off centre' by design, which is unsuitable for the world object grid this game is using.
- Node radius set to 0.5f.

===== 6/11/25 =====
Fixes:
- Fixed very intermittent bug (I hope) on game initialisation that could lead to the player not spawning and a stack overflow in some cases. I've been completely unable to replicate this bug reliably and honestly, just a shot in the dark. I think it's due to a circular reference somewhere but damned if I can find it. Just built in some better debugging to try and ID the issue if it does occur again and cleaned up a bunch of OnEnable code. After changes, haven't once seen the error.
- Specifically, changed Stat to do the check for default value and reset each time it's called rather than OnEnable. Unsure if this was causing the issue but given the way it calculates the Xplier, feels like a possible culprit. (Update from later in day: It was not).
- Issue may have been from Resource class. Resource would reference a ResourceItem, which would reference a GameObject prefab which had an attached ResourceItemInteract script, which referenced a Resource. Maybe this?
- Tidied up a heap of fields in the WorldSpawnData, marked the following as NonSerialized:
- : currentResourceDroppersTotal (int)
- : currentEnemyTotal (int)
- : boundaryPositions (List<Vector3>)
- Buffer Overflow hasn't occurred since... though, now that I've written this... we'll see.

Changes:
- Changed AI move to no longer use a coroutine for the movement calculations, this was just causing null ref errors on destroy, got sick of trying to fix it. Seems there's little point to continue using a CoRoutine when I have access to the Update function on the class.

===== 4/11/25 =====
Fixes:
- Fixed bug with ResourceItems not destroying themselves when they reached Consumers.
- Fixed bug where items were being 'Collected', rather than 'Consumed'.

- Fixed a bunch of bugs that fixing the last bug caused...
(including...)
- : Items were then immediately 'Collected' by the Player, if they were in range and had carry space.
- : Core would do the same thing! Hooray!
- : ResourceItems didn't move towards Consumers like they did Collectors.
- : (These were all because I'd forgotten to include checks for beingConsumed, so fixing the bug that used the incorrect 'beingCollected', suddenly exposed all the logic gaps where beingConsumed should have been accounted for but wasn't.

- Fixed rotation bug on Placeables when they spawn, due to lazy parenting.
- Fixed a bug where Constructors wouldn't do the check to spawn a placeable (and thus spawn one if they should) if the player had no stored total resources.
- Fixed bug with constructors only building 1 placeable.
- Fixed bug with player being able to collect multiple Placeables.

Changes:
- Added placeable functionality. These now spawn on Constructors.
- Added ability to pick up a Placeable.
- Added ability to 'place' placeable, spawns a building. Atm, this is just turrets, haha.
- Adjusted game camera, player size had changed slightly, was a bit off centre again on the Y.
