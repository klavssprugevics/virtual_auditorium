using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UIControl : NetworkBehaviour
{
    public GameObject[] userObjectList;
    public Text speakingInfoBox;

    void Start()
    {
        // Ideally should be updated when someone joins/leaves
        userObjectList = GameObject.FindGameObjectsWithTag("User");

    }
    void Update()
    {
        userObjectList = GameObject.FindGameObjectsWithTag("User");
        string speaking = "";
        print("Users in room: " + userObjectList.Length);
        foreach(GameObject user in userObjectList)
        {
            PlayerInfo info = user.GetComponent<PlayerInfo>();
            if(info.isTalking)
            {
                print(info.userName + " is talking...");
                speaking = speaking + info.userName + "\n"; 
            }
        }
        print(speaking);
        speakingInfoBox.text = speaking;
    }

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
