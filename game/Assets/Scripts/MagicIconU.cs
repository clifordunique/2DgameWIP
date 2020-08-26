using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicIconU : MonoBehaviour
{
    public static MagicIconU Instance { get; private set; }
    private int spellNo;
    Image image;

    public Sprite[] magicSpells;
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        image = GetComponent<Image>();
        image.enabled = true;
        image.sprite = magicSpells[Spells.Instance.currentSpell[0]];
    }

    public void UpdateIcon()
    {
        image.sprite = magicSpells[Spells.Instance.currentSpell[0]];
    }


}
