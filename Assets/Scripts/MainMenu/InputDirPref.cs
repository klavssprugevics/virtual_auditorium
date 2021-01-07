using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputDirPref : MonoBehaviour
{
    const string chosenDir = "DirPref";
    public InputField inputField;
    public Text dirLabel;
    public Button loadButton;

    void Start()
    {
        if(PlayerPrefs.HasKey(chosenDir))
        {
            inputField.text = PlayerPrefs.GetString(chosenDir);
            PlayerPrefs.DeleteKey(chosenDir);
        }
    }

    public void CheckField()
    {
        if(string.IsNullOrEmpty(inputField.text))
        {
            loadButton.interactable = false;
        }
        else
        {
            loadButton.interactable = true;
        }
    }

    public void SaveDir()
    {
        if(string.IsNullOrEmpty(inputField.text))
        {
            return;
        }

        dirLabel.text = inputField.text;
        PlayerPrefs.SetString(chosenDir, inputField.text);
    }
}
