using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerInfo : NetworkBehaviour
{
    [SyncVar]
    public string userName;

    [SyncVar]
    public Color color;

    void Start()
    {
        // Debug.Log("Created a new user with name: " + userName);
        gameObject.transform.Find("Name").gameObject.GetComponent<TextMesh>().text = userName;
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);

    }
}
