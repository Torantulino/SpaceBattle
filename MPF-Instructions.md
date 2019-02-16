## Multiplayer Framework
We're using **UNet** - Unity Networking together with High-Level API to handle all multiplayer aspects.  
All players in a scene are created using the same prefab - **Player**. To distinguish between "my" Player and other players in the scene we need an idea of **Local Player**.
The local player is set automatically when we *connect to a server* (it's useful to run a check if there's a reference before trying to access it).

### Features
* Create/connect to a server
* Synchronize position, rotation, hp and name of players
* Shoot from weapons
* Add and synchronize all parts that build up players' ships

### Scripts explanation
* *Controllers*
  * GameController.cs - provides some useful stuff
  * PlayerController.cs, PlayerControllerNet.cs - (partial class) controller for players
  * ProjectileController.cs - (temporary) class to control bullets
  * CameraController.cs - controls the Main Camera
  * CustomNetworkManager.cs - singleton for server/client matters
* *Destructibles*
  * Destructible.cs - destructible objects
  * Unit.cs (inherits from Destructible) - objects that can have parts
  * Ship.cs (inherits from Unit) - movable Units
* *Extensions*
  * GenericEventArgs.cs - implements generic version of EventArgs
  * TransformExtension.cs - adds DestroyChildren(..) extension method to Transform
* *Parts*
  * Part.cs - MonoBehaviour class for parts
  * Weapon.cs (inherits from Part) - Weapon MonoBehaviour - can turn and shoot
  * PartInfo.cs - ScriptableObject that holds information about a part (id, prefab)
  * PartData.cs - holds information about single Part that is attached to a Ship (id, position, rotation)
  * PartManager.cs - keeps a List of all PartInfo scriptable objects
  
### Using CustomNetworkManager (CNM)
> How to set up a scene to work with MP:  
> Drag all prefabs from Prefabs/Controllers folder to your scene.  

Default UI was removed.  
Hosting (creating a server and being a client at the same time)
```cs
NetworkClient CustomNetworkmanager.Instance.StartHost(int port = 7777)
```
Connecting to the server as a client
```cs
NetworkClient CustomNetworkmanager.Instance.Connect(string ipAddress = "localhost", int port = 7777)
```
You don't need to do anything with the return value. You can use events from CNM if you ever needed them.  
There's a lot of other methods in CNM, but please don't use them as they are just overridden callbacks from the parent class and I can't change their access rights to hide them. Most of them don't do anything now, but they might in the future.
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
```
### Properties
Please don't use private variables directly, to make sure that they're synchronized properly.
```cs
// PlayerController
string PlayerName
Ship Ship
ReadOnlyCollection<ItemContainer> Items

// Ship
float Hp
Vector3 Target
ReadOnlyCollection<PartData> PartsData
```
### Methods
```cs
// PlayerController
AddItem(int id, int quantity = 1)
RemoveItem(int id, int quantity = 1)
Refreshitems() // Not necessary and not recommended to use in most cases
ClearItems()

// Ship
void Thrust(Vector3 force)
void Shoot()
void RefreshParts()
void AddPart(PartData partData)

// Weapon
bool Ready()
```
### Events
For now, events use generic implementation of EventArgs (just use *Value* from the EventArgs).
```
// PlayerController
PlayerNameChanged
ItemsChanged

// Ship
HpChanged
PartsChanged
```
### Creating a new Part
1. Create a prefab for your Part and add a class that derives from Part to the parent GameObject of that prefab.
2. Fill all needed fields in Inspector.
3. Create ScriptableObject instance: Assets->Create->Parts->PartData
4. Edit all information in Inspector, make sure that id is unique.
5. Add it to the **Registered Parts** in the "Part Manager" component of PartManager.

**Never change fields of ScriptableObject instances via code - changes are permanent.**

Only server can add parts to Units. Any parts you add locally will be overridden when you refresh List for parts!
API for requesting Part adding will be provided soon.
### Player movement
Local Player movement isn't restricted in any way - all changes in position and rotation will be synchronized automatically. For now, there's `Thrust(Vector3 force)` method in Ship.
### Known issues
* when refreshing and rebuilding Parts for a Unit, each time *weapons* Count keeps rising until *2n - 1* (where *n* is actual number of weapons); there are no warnings/errors and Shoot() still works.
