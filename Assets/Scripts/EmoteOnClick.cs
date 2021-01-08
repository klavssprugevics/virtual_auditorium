using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmoteOnClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject user;

    public void OnPointerClick(PointerEventData eventData)
    {
        user.GetComponent<Emote>().HandleClick(this.name);
    }
}
