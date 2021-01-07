using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.UI;

/*
	Documentation: https://mirror-networking.com/docs/Components/NetworkManager.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class NetworkManagerExt : NetworkManager
{
    public struct UserInfo : NetworkMessage
    {
        public string name;
        public Color materialColor;
        public string directory;

    }


    public GameObject hostPrefab;
    public GameObject userPrefab;
    public GameObject colorPicker;
    public Text nameLabel;

    private bool[] seats = new bool[10];
    public Vector3[] seatPositions = new Vector3[10];

    #region Unity Callbacks

    public override void OnValidate()
    {
        base.OnValidate();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    #endregion

    #region Start & Stop

    /// <summary>
    /// Set the frame rate for a headless server.
    /// <para>Override if you wish to disable the behavior or set your own tick rate.</para>
    /// </summary>
    public override void ConfigureServerFrameRate()
    {
        base.ConfigureServerFrameRate();
    }

    /// <summary>
    /// called when quitting the application by closing the window / pressing stop in the editor
    /// </summary>
    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }

    #endregion

    #region Scene Management

    /// <summary>
    /// This causes the server to switch scenes and sets the networkSceneName.
    /// <para>Clients that connect to this server will automatically switch to this scene. This is called autmatically if onlineScene or offlineScene are set, but it can be called from user code to switch scenes again while the game is in progress. This automatically sets clients to be not-ready. The clients must call NetworkClient.Ready() again to participate in the new scene.</para>
    /// </summary>
    /// <param name="newSceneName"></param>
    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
    }

    /// <summary>
    /// Called from ServerChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows server to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    public override void OnServerChangeScene(string newSceneName) { }

    /// <summary>
    /// Called on the server when a scene is completed loaded, when the scene load was initiated by the server with ServerChangeScene().
    /// </summary>
    /// <param name="sceneName">The name of the new scene.</param>
    public override void OnServerSceneChanged(string sceneName) { }

    /// <summary>
    /// Called from ClientChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows client to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    /// <param name="sceneOperation">Scene operation that's about to happen</param>
    /// <param name="customHandling">true to indicate that scene loading will be handled through overrides</param>
    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling) { }

    /// <summary>
    /// Called on clients when a scene has completed loaded, when the scene load was initiated by the server.
    /// <para>Scene changes can cause player objects to be destroyed. The default implementation of OnClientSceneChanged in the NetworkManager is to add a player object for the connection if no player object exists.</para>
    /// </summary>
    /// <param name="conn">The network connection that the scene change message arrived on.</param>
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }

    #endregion

    #region Server System Callbacks

    /// <summary>
    /// Called on the server when a new client connects.
    /// <para>Unity calls this on the Server when a Client connects to the Server. Use an override to tell the NetworkManager what to do when a client connects to the server.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerConnect(NetworkConnection conn) {
        base.OnServerConnect(conn);


    }

    /// <summary>
    /// Called on the server when a client is ready.
    /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

    }

    /// <summary>
    /// Called on the server when a client adds a new player with ClientScene.AddPlayer.
    /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
    }

    /// <summary>
    /// Called on the server when a client disconnects.
    /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        GameObject clientInfo = conn.clientOwnedObjects.First().gameObject;
        int disconnectedSeat = clientInfo.GetComponent<PlayerInfo>().seatIndex;

        print("client sitting at seat index: " + disconnectedSeat + " has disconnected");

        seats[disconnectedSeat] = false;
        base.OnServerDisconnect(conn);
    }

    /// <summary>
    /// Called on the server when a network error occurs for a client connection.
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    /// <param name="errorCode">Error code.</param>
    public override void OnServerError(NetworkConnection conn, int errorCode) { }

    #endregion

    #region Client System Callbacks

    /// <summary>
    /// Called on the client when connected to a server.
    /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        float r = 0.5f;
        float g = 0.5f;
        float b = 0.5f;

        string dir = "";

        // Nolasa krasu no atminas
        if(PlayerPrefs.HasKey("red"))
        {
            r = float.Parse(PlayerPrefs.GetString("red"));
        }

        if(PlayerPrefs.HasKey("green"))
        {
            g = float.Parse(PlayerPrefs.GetString("green"));
        }

        if(PlayerPrefs.HasKey("blue"))
        {
            b = float.Parse(PlayerPrefs.GetString("blue"));
        }

        if(PlayerPrefs.HasKey("DirPref"))
        {
            dir = PlayerPrefs.GetString("DirPref");
        }

        UserInfo userInfo = new UserInfo
        {
            name = nameLabel.text,
            materialColor = new Color(r, g, b, 1f),
            directory = dir
        };

        Debug.Log("Creating a new player: " + userInfo.name);

        conn.Send(userInfo);
    }

    /// <summary>
    /// Called on clients when disconnected from a server.
    /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {

        base.OnClientDisconnect(conn);
    }

    /// <summary>
    /// Called on clients when a network error occurs.
    /// </summary>
    /// <param name="conn">Connection to a server.</param>
    /// <param name="errorCode">Error code.</param>
    public override void OnClientError(NetworkConnection conn, int errorCode) { }

    /// <summary>
    /// Called on clients when a servers tells the client it is no longer ready.
    /// <para>This is commonly used when switching scenes.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientNotReady(NetworkConnection conn) { }

    #endregion

    #region Start & Stop Callbacks

    // Since there are multiple versions of StartServer, StartClient and StartHost, to reliably customize
    // their functionality, users would need override all the versions. Instead these callbacks are invoked
    // from all versions, so users only need to implement this one case.

    /// <summary>
    /// This is invoked when a host is started.
    /// <para>StartHost has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartHost() { }

    /// <summary>
    /// This is invoked when a server is started - including when a host is started.
    /// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartServer() {
        base.OnStartServer();

        NetworkServer.RegisterHandler<UserInfo>(OnCreateUser);
    }

    void OnCreateUser(NetworkConnection conn, UserInfo message)
    {
        GameObject gameObject;

        // Lai hosts izskatas savadak par user
        if(numPlayers == 0)
        {
            gameObject = Instantiate(hostPrefab);
        }
        else
        {
            gameObject = Instantiate(userPrefab);
        }

        // Iedod player info
        PlayerInfo player = gameObject.GetComponent<PlayerInfo>();
        player.userName = message.name;
        player.color = message.materialColor;


        Transform position = gameObject.GetComponent<Transform>();

        int playerCount = NetworkServer.connections.Count;

        if(playerCount == 1)
        {
            // Tikai hosts pievienojies
            position.position = new Vector3(8.391f, 2.158f, 7.466f);
        }
        else
        {
            // Mekle kamer atrod brivu vietu
            int yourSeat = 0;
            while(seats[yourSeat])
                yourSeat++;

            position.position = seatPositions[yourSeat];
            seats[yourSeat] = true;
            player.seatIndex = yourSeat;
        }



        print("Player count: " + NetworkServer.connections.Count);
        NetworkServer.AddPlayerForConnection(conn, gameObject);
    }


    /// <summary>
    /// This is invoked when the client is started.
    /// </summary>
    public override void OnStartClient() {
    }

    /// <summary>
    /// This is called when a host is stopped.
    /// </summary>
    public override void OnStopHost() { }

    /// <summary>
    /// This is called when a server is stopped - including when a host is stopped.
    /// </summary>
    public override void OnStopServer() { }

    /// <summary>
    /// This is called when a client is stopped.
    /// </summary>
    public override void OnStopClient() { }

    #endregion
}
