using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace Photon.Voice.Unity.UtilityScripts
{
    public class PushToTalk : MonoBehaviour
    {
        public Recorder recorder;

        void Start()
        {
            recorder = this.GetComponent<Recorder>();
        }

        void Update()
        {
            if(Input.GetKey("t"))
            {
                recorder.TransmitEnabled = true;
            }
            else
            {
                recorder.TransmitEnabled = false;
            }
        }

    }
}