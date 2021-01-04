using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RGBColorPicker : MonoBehaviour
{
    Transform imagePreview;

    private float r = 0.5f;
    private float g = 0.5f;
    private float b = 0.5f;

    private bool valuesLoaded = false;
    void Start()
    {
        
        imagePreview = gameObject.transform.Find("ColorPreview");


        if(PlayerPrefs.HasKey("red"))
        {
            r = float.Parse(PlayerPrefs.GetString("red"));

        }

        if(PlayerPrefs.HasKey("green"))
        {
            g = float.Parse(PlayerPrefs.GetString("green"));
        }

        if(PlayerPrefs.HasKey("blue"))
        {
            b = float.Parse(PlayerPrefs.GetString("blue"));
        }

        imagePreview.GetComponent<Image>().color = new Color(r, g, b, 1f);

        gameObject.transform.Find("RSlider").GetComponent<Slider>().value = r;
        gameObject.transform.Find("GSlider").GetComponent<Slider>().value = g;
        gameObject.transform.Find("BSlider").GetComponent<Slider>().value = b;

        valuesLoaded = true;
    }

    public void setColor()
    {
        if(!valuesLoaded)
            return;

        r = gameObject.transform.Find("RSlider").GetComponent<Slider>().value;
        g = gameObject.transform.Find("GSlider").GetComponent<Slider>().value;
        b = gameObject.transform.Find("BSlider").GetComponent<Slider>().value;
        
        imagePreview.GetComponent<Image>().color = new Color(r, g, b, 1);

    }

    public void saveColor()
    {
        Debug.Log("Color saved: " + r + g + b);
        PlayerPrefs.SetString("red", r.ToString());
        PlayerPrefs.SetString("green", g.ToString());
        PlayerPrefs.SetString("blue", b.ToString());
    }
}
