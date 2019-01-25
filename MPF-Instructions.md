## Multiplayer Framework
We're using **UNet** - Unity Networking together with High Level API to handle all multiplayer aspects.  
All players in a scene are created using the same prefab - **Player**. To distinguish between "my" Player and other players in the scene we need an idea of **Local Player**.
Local player is set automatically when we *connect to a server* (it's useful to run a check if there's a reference before trying to access it).  
**GameController** is a singleton MonoBehaviour class that provides some useful stuff.

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
### Properties & Methods
Please don't use private variables directly, to make sure that they're synchronized properly. Even better - use events instead of getting values.
Properties
```cs
PlayerController.PlayerName
PlayerController.Ship
Ship.Hp
```
Methods
```cs

```
### Events
For now, events use generic implementation of EventArgs in the GameController.cs file (just use *Value* from the EventArgs). Example:
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
