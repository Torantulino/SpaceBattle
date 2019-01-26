## Multiplayer Framework
We're using **UNet** - Unity Networking together with High Level API to handle all multiplayer aspects.  
All players in a scene are created using the same prefab - **Player**. To distinguish between "my" Player and other players in the scene we need an idea of **Local Player**.
Local player is set automatically when we *connect to a server* (it's useful to run a check if there's a reference before trying to access it).

### Features
* Create/connect to a server
* Synchronize position, rotation, hp and name of players
* Shoot from weapons
* Add and synchronize all parts that build up players' ships

### Scripts explanation
* GameController - provides some useful stuff
* PlayerController.cs, PlayerControllerNet.cs - (partial class) controller for players
* ProjectileController - (temporary) class to control bullets
* Destructible.cs - destructible objects
* Unit.cs (inherits from Destructible) - objects that can have parts
* Ship.cs (inherits from Unit) - movable Units
* *Extensions*
  * TransformExtension.cs - adds DestroyChildren(..) extension method to Transform
* *Parts*
  * Part.cs - MonoBehaviour class for parts
  * Weapon.cs (inherits from Part) - Weapon MonoBehaviour - can turn and shoot
  * PartInfo.cs - ScriptableObject that holds information about a part (id, prefab)
  * PartData.cs - holds information about single Part that is attached to a Ship (id, position, rotation)
  * PartController.cs - keeps a List of all PartInfo scriptable objects

### Accessing Local Player
To access local player use static fields in the GameController, for  **GameObject**
```cs
GameController.LocalPlayer
```
Local player **PlayerController** script (with all information about player, ex. name)
Player controller is attached to all Player objects, so for example you can access any player name by using `GetComponent<PlayerController>().PlayerName`
```cs
GameController.LocalPlayerController
```
Local player **Ship** script (all information about vessel, ex. hp)
```
GameController.LocalPlayerController.Ship
// or
GameController.LocalPlayer.GetComponent<Ship>()
```
### Properties
Please don't use private variables directly, to make sure that they're synchronized properly.
```cs
// PlayerController
PlayerName
Ship

// Ship
Hp
Target
```
### Methods
```cs
// PlayerController

// Ship
void Thrust(Vector3 force)
void Shoot()
void RefreshParts()

// Weapon
bool Ready()
```
### Events
For now, events use generic implementation of EventArgs in the GameController.cs file (just use *Value* from the EventArgs).
```
// PlayerController
PlayerNameChanged

// Ship
HpChanged
PartsChanged
```

### Creating a new Part
1. Create a prefab and add a class that derives from Part to the parent object.
2. Fill all needed visible fields in the inspector for it.
3. Create ScriptableObject instance: Assets/Create/Parts/*
4. Edit all information in Inspector, make sure that id is unique.
5. Add it to the **Registered Parts** in the "Part Controller" component of GameController.

**Never change fields of ScriptableObject instances via code - changes are permanent.**

Only server can add parts to Units. Any parts you add locally will be overriden when you refresh List for parts!
API for requesting Part adding will be provided soon.

### Player movement
Local Player movement isn't restricted in any way - all changes in position and rotation will be synchronized automatically. For now, there's `Thrust(Vector3 force)` method in Ship.
