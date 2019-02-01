using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using PlayerController = UnityEngine.Networking.PlayerController;

public class CustomNetworkManager : NetworkManager
{
    #region Singleton
    private static CustomNetworkManager instance;

    public static CustomNetworkManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    #region Events

    /// <summary>
    /// Client connected to the server.
    /// </summary>
    public event EventHandler<EventArgs> ServerConnect;
    /// <summary>
    /// Client disconnected from the server.
    /// </summary>
    public event EventHandler<EventArgs> ServerDisconnect;

    /// <summary>
    /// Connected to the server.
    /// </summary>
    public event EventHandler<EventArgs> ClientConnect;
    /// <summary>
    /// Disconnected from the server.
    /// </summary>
    public event EventHandler<EventArgs<NetworkConnection>> ClientDisconnect;

    #endregion

    #region Custom methods

    public override NetworkClient StartHost()
    {
        SetPort();
        return base.StartHost();
    }

    /*public void StartHost()
    {
        Debug.Log("Start HOSTTT");
        SetPort();
        base.StartHost();
        //NetworkManager.singleton.StartHost();
    }*/

    public void Connect()
    {
        SetIpAddress();
        SetPort();
        StartClient();

        //todo temp
        if (isNetworkActive)
        {
            StopHost();
            //NetworkManager.singleton.StopClient();
        }

    }

    public void SetIpAddress()
    {
        string ipAddress = "";
        if (string.IsNullOrEmpty(ipAddress))
            ipAddress = "localhost";
        networkAddress = ipAddress;
    }

    public void SetPort()
    {
        networkPort = 7777;
    }

    #endregion

    // Server callbacks

    public override void OnServerConnect(NetworkConnection conn)
    {
        if(ServerConnect != null)
            ServerConnect(this, EventArgs.Empty);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkServer.DestroyPlayersForConnection(conn);

        if (conn.lastError != NetworkError.Ok)
            if (LogFilter.logError)
                Debug.LogError("ServerDisconnected due to error: " + conn.lastError);

        if (ServerDisconnect != null)
            ServerDisconnect(this, EventArgs.Empty);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        NetworkServer.SetClientReady(conn);

        Debug.Log("Client is set to the ready state (ready to receive state updates): " + conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        Debug.Log("Client has requested to get his player added to the game");

        // Refreshing parts for all players
        foreach (UnityEngine.Networking.PlayerController playerController in client.connection.playerControllers)
        {
            playerController.gameObject.GetComponent<Unit>().RefreshParts();
        }
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, UnityEngine.Networking.PlayerController player)
    {
        if (player.gameObject != null)
            NetworkServer.Destroy(player.gameObject);
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("Server network error occurred: " + (NetworkError)errorCode);
    }

    public override void OnStartHost()
    {
        Debug.Log("Host has started");
    }

    public override void OnStartServer()
    {
        Debug.Log("Server has started");
    }

    public override void OnStopServer()
    {
        Debug.Log("Server has stopped");
    }

    public override void OnStopHost()
    {
        Debug.Log("Host has stopped");
    }

    // Client callbacks

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        if (ClientConnect != null)
            ClientConnect(this, EventArgs.Empty);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        StopClient();

        if (conn.lastError != NetworkError.Ok)

            if (LogFilter.logError)
                Debug.LogError("ClientDisconnected due to error: " + conn.lastError);

        if (ClientDisconnect != null)
            ClientDisconnect(this, new EventArgs<NetworkConnection>(conn));
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("Client network error occurred: " + (NetworkError)errorCode);
    }

    public override void OnClientNotReady(NetworkConnection conn)
    {
        Debug.Log("Server has set client to be not-ready (stop getting state updates)");
    }

    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log("Client has started");
    }

    public override void OnStopClient()
    {
        Debug.Log("Client has stopped");
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);

        Debug.Log("Server triggered scene change and we've done the same, do any extra work here for the client...");
    }

}
