using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class InputUsername : NetworkBehaviour
{
    // Saglabas lietotaju vardu ka unity key
    const string userName = "UserName";
    public Text nameLabel;
    public Text nameLabel2;

    void Start()
    {
        string defaultName = string.Empty;
        InputField inputField = this.GetComponent<InputField>();

        if(PlayerPrefs.HasKey(userName))
        {
            defaultName = PlayerPrefs.GetString(userName);
            inputField.text = defaultName;
        }
    }


    public void SaveUsername(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            return;
        }

        nameLabel.text = value;
        nameLabel2.text = value;
        PlayerPrefs.SetString(userName, value);
    }

}
