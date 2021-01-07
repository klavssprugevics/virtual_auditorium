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
    public bool isTalking;

    [SyncVar]
    public Color color;

    public string directory;

    public Renderer shirt;

    public int seatIndex;

    void Start()
    {
        Debug.Log("Created a new user with name: " + userName + ".");
        gameObject.transform.Find("Name").gameObject.GetComponent<TextMesh>().text = userName;
        // gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
        shirt.material.SetColor("_Color", color);
        isTalking = false;

    }
}
