# Daily Golf Project
Unity example project demonstrating modular project architecture. A tiny golf game on a procedural map.

The goal of this game is to use data-oriented design patterns and modular coding patterns to allow the different systems to exist independently, and be built up over time.

## Game Architecture Patterns Being Used
- Data-Oriented Design. Primarily through storing game data in ScriptableObjets. See the [still-relevant 2017 Unite talk from Ryan Hipple](https://www.youtube.com/watch?v=raQ3iHhE_Kk).
- Actions. Using events ([static](https://guidebook.hdyar.com/docs/programming/advanced/static-objects-and-unity/) or on scriptableObjects) to decouple dependencies. See my [Event Systems in Unity](https://guidebook.hdyar.com/docs/programming/architecture/event-systems/) page.
- Extension Methods. Great for reusability, but also great for plain old readability. e.g. [self-documenting](https://en.wikipedia.org/wiki/Self-documenting_code) code.

## System Notes
### Golf Ball Movement
'Stroke' is a [POCO](https://en.wikipedia.org/wiki/Plain_old_CLR_object) that describes a single hit on the golf ball. It's used to store previous hits and edited at runtime. GolfMovement.CurrentStroke is what the trajectory prediction system is using to figure out what force might get added, for example.

The 'Caddy' is a scriptableObject (ActiveGolfConfiguration) that stores the current golfing setup: the clubs, etc.

### UI
HUD reads from the 'caddy'. With the scriptable object, it is completely independent from any other element.
in-scene UI is handled by the trajectory prediction system.

### Trajectory Prediction
Basically entirelly in a single script/child of the player, GolfHitPreviewLine.cs.

Uses [multi-scene physics](https://docs.unity3d.com/Manual/physics-multi-scene.html) to simulate the balls path and draw a line for each tick of that simulation. See the [TNTC](https://www.youtube.com/watch?v=4VUmhuhkELk) video for a breakdown of the technique.

### Custom Attributes
[ReadOnly] and [Layer] are not attributes that are built into Unity (although they should be).
I implemented these as custom attributes, see the utilities folder. Each one is in it's own folder/namespace because I imagine you may want to directly copy them into your own projects. Go for it. That's what I do.

### Map Generation
todo. Will be pulling from techniques used in my [2DRoguelikeLevelGenerator](https://github.com/hunterdyar/2DRougelikeLevelGenerator/) project.
