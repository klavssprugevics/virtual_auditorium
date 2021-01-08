using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Chat : NetworkBehaviour
{
    public InputField inputField;
    public Text message;


    public void OpenChat()
    {
        print("User opened chat");
        inputField.ActivateInputField();
    }

    public void SendMessage(string msg)
    {
        print("User sent message");
        message.text = msg;
    }

}
