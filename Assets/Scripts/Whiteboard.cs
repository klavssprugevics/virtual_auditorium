using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Mirror;

public class Whiteboard : NetworkBehaviour
{
    private Texture2D[] imageList;

    [SyncVar]
    private int imageIndex;

    private Renderer[] screens; 

    public string pictureDir;
    private string[] files;
    private string dirPrefix;
    private bool imagesLoaded;

    public Texture2D activeTexture;
    private Texture2D receivedTexture;
    

    // Start is called before the first frame update
    void Start()
    {

        // Atlasis visus GameObjects, kam ir tags Screen, un tiem liks virsu attelu
        GameObject[] listOfScreenObjects = GameObject.FindGameObjectsWithTag("Screen");
        screens = new Renderer[listOfScreenObjects.Length];

        // Izveido sarakstu kura uzglabas sos objektus
        for(int tempCounter = 0; tempCounter < listOfScreenObjects.Length; tempCounter++)
        {
            screens[tempCounter] = listOfScreenObjects[tempCounter].GetComponent<Renderer>();
        }

        // Ja lietotajs ir istabas host, tad vins izvelas direktoriju ar atteliem
        if(isServer && isClient)
        {
            imageIndex = 0;

            if(PlayerPrefs.HasKey("DirPref"))
            {
                // pictureDir = @"C:\Users\Klavs\Pictures\virtual_aud";
                pictureDir = PlayerPrefs.GetString("DirPref");
                dirPrefix = @"file://";
                files = System.IO.Directory.GetFiles(pictureDir, "*.jpg");
                StartCoroutine(LoadImages());
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(!imagesLoaded)
            return;

        // Ar q uzliek ieprieksejo slaidu
        // ar e uzliek nakamo slaidu
        // Tikai hosts var mainit slaidus
        if(isServer && isClient)
        {
            if(Input.GetKeyDown("e"))
            {
                if(imageIndex < imageList.Length - 1)
                {
                    imageIndex++;
                    activeTexture = imageList[imageIndex];

                    // Nosuta visiem client jauno slaidu
                    RpcSlide(activeTexture.EncodeToJPG());
                }
            }
            else if(Input.GetKeyDown("q"))
            {
                if(imageIndex > 0)
                {
                    imageIndex--;
                    activeTexture = imageList[imageIndex];

                    // Nosuta visiem client jauno slaidu
                    RpcSlide(activeTexture.EncodeToJPG());
                }
            }
        }
    }

    //https://answers.unity.com/questions/25271/how-to-load-images-from-given-folder.html
    // Izveido sarakstu ar textures, no atteliem
    IEnumerator LoadImages()
    {
        imageList = new Texture2D[files.Length];

        int counter = 0;
        foreach(string fname in files)
        {
            // Ielasa attelu un izveido texture
            string pathTemp = dirPrefix + fname;
            WWW www = new WWW(pathTemp);

            Texture2D texTmp = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            www.LoadImageIntoTexture(texTmp);

            // Pievieno sarakstam
            imageList[counter] = texTmp;
            counter++;

            yield return fname;
            InitializeSlides();
        }
    }
    
    void InitializeSlides()
    {
        if(imageList.Length < 1)
            return;

        // print("Image loading finished, found " + imageList.Length + " slides");
        activeTexture = imageList[imageIndex];

        // Inicialize sakuma slaidu
        foreach(var scr in screens)
        {
            scr.material.mainTexture = activeTexture;
        }

        imagesLoaded = true;
    }


    public override void OnStartAuthority()
    {
        if(isServer)
            return;

        CmdGetActiveSlide();
    }

    [Command]
    public void CmdGetActiveSlide()
    {
        print("Client called command! Sending with index: " + imageIndex);
        RpcSlide(activeTexture.EncodeToJPG());
    }

    [ClientRpc]
    void RpcSlide(byte[] recievedByte)
    {
        Texture2D recievedSlide = new Texture2D(1, 1);
        recievedSlide.LoadImage(recievedByte);
        activeTexture = recievedSlide;

        foreach(var scr in screens)
        {
            scr.material.mainTexture = activeTexture;
        }
    }
}
