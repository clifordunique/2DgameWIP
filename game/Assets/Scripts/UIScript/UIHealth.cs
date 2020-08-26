using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public static UIHealth Instance { get; private set; }
    
    public Image bar;

    public GameObject healthBar;
    Animator healthBarAnimator;

    float originalSize;

    // Use this for initialization
    void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        healthBarAnimator = healthBar.GetComponent<Animator>();
    }
    void OnEnable()
    {
        originalSize = bar.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
        if (value <= 0.25f)
        {
            healthBarAnimator.SetBool("LowHealth", true);
        }
        else
        {
            healthBarAnimator.SetBool("LowHealth", false);
        }
    }
}
