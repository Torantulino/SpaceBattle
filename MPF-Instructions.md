## Multiplayer Framework
We're using **UNet** - Unity Networking together with High Level API to handle all multiplayer aspects.  
All players in a scene are created using the same prefab - **Player**. To distinguish between "my" Player and other players in the scene we need an idea of **Local Player**.
Local player is set automatically when we *connect to a server* (it's useful to run a check if there's a reference before trying to access it).  
**GameController** is a singleton class that provides some useful stuff.

### Accessing Local Player
To access local player use static fields in the GameController, for  **GameObject**
```cs
GameController.LocalPlayer
```
Local player **PlayerController** script (with all information about player, ex. name)
Player controller is attached to all Player objects, so for example you can access any player name by using `GetComponent<PlayerController>().GetPlayerName()`
```cs
GameController.LocalPlayerController
```
Local player **Ship** script (all information about vessel, ex. hp)
```
GameController.LocalPlayerController.Ship
// or
GameController.LocalPlayer.GetComponent<Ship>()
```
### Get & Set methods
Due to UNet limitations all variables that are synchronized, ex. Ship's Hp cannot use *get* and *set* properties. For them there are Get and Set methods and they handle all code needed for synchronization.
```cs
PlayerController.GetPlayerName();
PlayerController.SetPlayerName(string newName);
Ship.GetHp();
```
### Events
There're events everywhere and events are nice, so use them. For now they use generic implementation of EventArgs in the GameController.cs file (just use *Value* from the EventArgs). Example:
```cs
GameController.LocalPlayerController.PlayerNameChanged += (s, e) => 
Debug.Log("Local player name was changed to: " + e.Value);
```
Events:
```
* PlayerController:
PlayerNameChanged

* Ship:
HpChanged
```
### Player movement
Local Player movement isn't restricted in any way - all changes in position and rotation will be synchronized automatically. For now, there's `Thrust(Vector3 force)` method in Ship.
