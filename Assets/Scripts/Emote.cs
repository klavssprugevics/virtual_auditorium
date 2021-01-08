using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Emote : NetworkBehaviour
{
    public GameObject emote;
    public GameObject emotePanel;

    public Material thUp;
    public Material thDown;
    public Material rh;
    public Material smile;


    private bool choosingEmote = false;

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer)
        {
            if(Input.GetKeyDown("tab"))
            {
                choosingEmote = !choosingEmote;
            }
                // choosingEmote = !choosingEmote;
                
            if(choosingEmote)
            {
                emotePanel.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }


        // emotePanel.SetActive(false);
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    
    }

    public void HandleClick(string name)
    {
        choosingEmote = false;
        emotePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CmdSendEmote(this.GetComponent<NetworkIdentity>().netId, name);
    }

    [Command]
    public void CmdSendEmote(uint id, string name)
    {
        RpcUpdateEmote(id, name);
    }

    [ClientRpc]
    public void RpcUpdateEmote(uint id, string name)
    {
        print("Setting gameobject emote with active with id: " + id);

        // Atrod gamemobject ar attiecigo networkId
        GameObject[] userList = GameObject.FindGameObjectsWithTag("User");

        foreach(var user in userList)
        {
            if(user.GetComponent<NetworkIdentity>().netId == id)
            {
                Transform emote = user.transform.Find("Emote");

                Material newEmote = thUp;

                switch(name)
                {
                    case "ThumbsUp":
                        newEmote = thUp;
                        break;
                    case "ThumbsDown":
                        newEmote = thDown;
                        break;
                    case "Smile":
                        newEmote = smile;
                        break;
                    case "RaisedHand":
                        newEmote = rh;
                        break;
                }

                emote.GetComponent<Renderer>().material = newEmote;
                emote.gameObject.SetActive(true);
                StartCoroutine(emoteTimeout(emote));
            }
        }
    }



    private IEnumerator emoteTimeout(Transform emote)
    {
        yield return new WaitForSeconds(5);
        emote.gameObject.SetActive(false);
    }
}
