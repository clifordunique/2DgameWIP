using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeedLevel : MonoBehaviour
{
    public static UISpeedLevel Instance { get; private set; }

    public Image bar;

    float originalSize;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }


    public void SetValue(float value)
    {
        bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize * value);
    }
}
