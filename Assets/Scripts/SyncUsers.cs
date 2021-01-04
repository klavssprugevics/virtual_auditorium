using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncUsers : NetworkBehaviour
{
    private TextMesh nameLabel;
    private Transform User;


    // https://forum.unity.com/threads/how-to-get-player-names-sync.350910/

    // [SyncVar]
    // public string username;

    // public override void OnStartLocalPlayer()
    // {
    //     SetName();
    // }

    // [Client]
    // void SetName()
    // {
    //     username = PlayerPrefs.GetString("UserName");
    //     CmdSendName(username);
    // }

    // [Command]
    // void CmdSendName(string name)
    // {
    //     RpcSetName(name);
    // }

    // [ClientRpc]
    // void RpcSetName(string name)
    // {
    //     Debug.Log(name);
    //     nameLabel.text = name;
    // }

    // void Start()
    // {
    //     User = GetComponent<Transform>();
    //     nameLabel = User.Find("Name").gameObject.GetComponent<TextMesh>();

    // }

}
