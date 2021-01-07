using System.Collections;
using System.Collections.Generic;
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

    public NetworkIdentity networkIdentity;
    public int MovementSpeed = 5;
    public int Sensitivity = 1;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        User = GetComponent<Transform>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        
    }

    void Update()
    {

        if(isLocalPlayer)
        {
            CameraLook();
            Move();
            info = User.GetComponent<PlayerInfo>();

            if(Input.GetKey("t"))
            {
                CmdIsTalk(info);
            }
            else
            {
                CmdIsNotTalk(info);
            }

        }
        else
        {
            cameraTransform.gameObject.SetActive(false);
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
