using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UserControl : NetworkBehaviour
{
    private CharacterController characterController;
    private Transform cameraTransform;
    private Transform User;
    private Vector3 Position;
    private PlayerInfo info;

    // Kameru references
    private Camera fpCamera;
    private Camera audCamera;
    private Camera screenCamera;

    public NetworkIdentity networkIdentity;
    public int MovementSpeed = 5;
    public int Sensitivity = 1;

    private bool inputLock = false;

    // Chatam
    // public GameObject chatPrefab;
    public GameObject chat;
    public InputField inputField;
    public Text inputText;
    public Text chatBox;
    public Text placeHolderText;

    private bool typing = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();
        User = GetComponent<Transform>();
        fpCamera = GetComponentInChildren<Camera>();
        cameraTransform = fpCamera.transform;

        GameObject[] cameraList = GameObject.FindGameObjectsWithTag("Camera");

        // Atrod kameras instances 
        foreach(var camera in cameraList)
        {
            if(camera.name == "MainCamera")
            {
                audCamera = camera.GetComponent<Camera>();
            }
            if(camera.name == "ScreenCamera")
            {
                screenCamera = camera.GetComponent<Camera>();
            }
        }

            // Atrod chata instanci

        if(!isLocalPlayer)
            chat.SetActive(false);
    }

    void Update()
    {

        if(isLocalPlayer)
        {

            info = User.GetComponent<PlayerInfo>();

            if(!inputLock)
            {
                CameraLook();
                // Move();

                // Push to talk
                if(Input.GetKey("t"))
                {
                    CmdIsTalk(info);
                }
                else
                {
                    CmdIsNotTalk(info);
                }

                // Maina kameras
                if(Input.GetKeyDown("1"))
                {
                    print("Switching to first person camera");
                    screenCamera.enabled = false;
                    audCamera.enabled = false;
                    fpCamera.enabled = true;
                }
                else if(Input.GetKeyDown("2"))
                {
                    print("Switching to auditorium camera");
                    fpCamera.enabled = false;
                    screenCamera.enabled = false;
                    audCamera.enabled = true;
                }
                else if(Input.GetKeyDown("3"))
                {
                    print("Switching to screen camera");
                    fpCamera.enabled = false;
                    audCamera.enabled = false;
                    screenCamera.enabled = true;
                }
            }

            // Lock-unlock kursoru
            if(Input.GetKeyDown(KeyCode.Escape) && !typing)
            {
                switch(Cursor.lockState)
                {
                    case CursorLockMode.None:
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                        inputLock = false;
                        break;
                    case CursorLockMode.Locked:
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        inputLock = true;
                        break;
                }

            }
            else if(Input.GetKeyDown(KeyCode.Escape) && typing)
            {
                inputField.DeactivateInputField();
                placeHolderText.text = "Press Y to chat";
            }

            // Chat
            if(Input.GetKeyDown("y"))
            {
                inputField.ActivateInputField();
                inputLock = true;
                typing = true;
                placeHolderText.text = "";
            }
            if(typing)
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    CmdSendMessage(info.userName + ": " + inputText.text);
                    inputLock = false;
                    typing = false;
                    inputField.text = "";
                    inputField.DeactivateInputField();
                    placeHolderText.text = "Press Y to chat";
                }
            }

        }
        else
        {
            cameraTransform.gameObject.SetActive(false);
        }
    }

    [Command]
    public void CmdSendMessage(string msg)
    {
        RpcRecieveMessage(msg);
    }

    [ClientRpc]
    public void RpcRecieveMessage(string msg)
    {
        print("New message to write: " + msg);

        // So var labak izdarit, bet nevaru atrast kapec neupdatojas uz visiem locally
        foreach(var ch in GameObject.FindGameObjectsWithTag("Chat"))
        {
            Text instance = ch.GetComponent<Text>();
            string currBox = instance.text + msg + "\n";
            string[] split = currBox.Split('\n');
            
            // Ja vairak par 7 zinam tad izdesh vecako
            if(split.Length > 8)
                split = split.Where((item, index) => index != 0).ToArray();

            instance.text = string.Join("\n", split);
        }

    }

    [Command]
    public void CmdIsTalk(PlayerInfo info)
    {
        info.isTalking = true;
    }

    [Command]
    public void CmdIsNotTalk(PlayerInfo info)
    {
        info.isTalking = false;
    }

    void CameraLook()
    {
        // Seko lidzi peles kustibai
        float horizontal = Input.GetAxis("Mouse X") * Sensitivity;
        float vertical = Input.GetAxis("Mouse Y") * Sensitivity;

        // Horizontali kustinot peliti, lietotajs tiek rotets attiecigaja virziena
        User.Rotate(0f, horizontal, 0f);

        // Vertikali kustinot peliti, kustinata tiek tikai kamera
        cameraTransform.transform.Rotate(-vertical, 0f, 0f);

        // Ierobezo cik augsti var kustinat kameru
        if (cameraTransform.transform.rotation.eulerAngles.x > 180f && cameraTransform.transform.rotation.eulerAngles.x < 280f)
        {
            // Ja kamera tiek kustinata par augstu, tad vienkarsi atstaj to pie max
            float currentYrotation = cameraTransform.transform.eulerAngles.y;
            cameraTransform.transform.rotation = Quaternion.Euler(280f, currentYrotation, 0f);
        }

        // Ierobezo cik zemu var kustinat kameru
        if (cameraTransform.transform.rotation.eulerAngles.x > 60f && cameraTransform.transform.rotation.eulerAngles.x < 180f)
        {
            // Ja kamera tiek kustinata par zemu, tad vienkarsi atstaj to pie min
            float currentYrotation = cameraTransform.transform.eulerAngles.y;
            cameraTransform.transform.rotation = Quaternion.Euler(60f, currentYrotation, 0f);
        }

            // Paslepj peliti centra, lai nevar iziet arpus loga 
            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;  
    }


    void Move()
    {
        Position = (transform.forward * Input.GetAxis("Vertical") * MovementSpeed) + (transform.right * Input.GetAxis("Horizontal") * MovementSpeed);
        characterController.Move(Position * Time.deltaTime); 
    }
}
