using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UIControl : NetworkBehaviour
{

    public void LeaveRoom()
    {
        if(NetworkManager.singleton.mode == NetworkManagerMode.Host)
        {
            NetworkManager.singleton.StopHost();
        }

        if(NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
