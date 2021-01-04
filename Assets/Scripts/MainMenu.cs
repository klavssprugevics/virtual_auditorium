using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager = null;
    [SerializeField] private InputField ipInput = null;


    public void HostLobby()
    {
        networkManager.StartHost();
    }

    public void JoinLobby()
    {
        string ipAddress = ipInput.text;
        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();
    }
}
