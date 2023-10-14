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

### Input
Uses new input system. InputActins have a c# class generated, and interfacing with the input actions is done entirely by an InputReader scriptable object. THe rest of the code base only interfaces with this, which provides convenient actions, process functions, and read-only properties. It keeps the rest of the codebase agnostic of which input system we are using. It lets us send 'fake' inputs in a non-jank way easily, either with inspector scripts or with public functions.

Using a scriptableobject to "wrap" an inputactions has the further advantages of being a convenient place to store input settings, and a place to put custom inspector doodads to preview/read the data for debugging, and fire off test actions.  The disadvantages are that it's a little overkill for this project, a public reference to inputaction assets is certainly fine for a game of this scale. Or, we could do it the same, but provide the data as static actions and floats! I don't use statics because it lets me use multiple scriptable objects to store different settings instances for testing and swapping out easily. Especially useful for a project in source control.

Really, I just like this method for the sake of the rest of the code base. Doing a lot of XR development, it's always useful to have test buttons in the scene (although not needed for a project like this one). I like completely compartmentalizing input away - it tends to be a place where complexity grows over time, and lots of little input handling scripts has always been a headache.

### UI
HUD reads from the 'caddy'. With the scriptable object, it is completely independent from any other element.
in-scene UI is handled by the trajectory prediction system.

### Trajectory Prediction
Basically entirelly in a single script/child of the player, GolfHitPreviewLine.cs.

Uses [multi-scene physics](https://docs.unity3d.com/Manual/physics-multi-scene.html) to simulate the balls path and draw a line for each tick of that simulation. See the [TNTC](https://www.youtube.com/watch?v=4VUmhuhkELk) video for a breakdown of the technique.

### Camera Control
Currently I use Cinemachine. The FreeLook camera is doing most of the work for me. 
A todo will be better (more precise) aiming with a 'dead zone' before the camera starts panning. But I don't really know how I want the camera input to work yet.

### Custom Attributes
[ReadOnly] and [Layer] are not attributes that are built into Unity (although they should be).
I implemented these as custom attributes, see the utilities folder. Each one is in it's own folder/namespace because I imagine you may want to directly copy them into your own projects. Go for it. That's what I do.

### Map Generation
todo. Will be pulling from techniques used in my [2DRoguelikeLevelGenerator](https://github.com/hunterdyar/2DRougelikeLevelGenerator/) project.
