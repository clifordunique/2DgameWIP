using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicIconU : MonoBehaviour
{
    public static MagicIconU Instance { get; private set; }
    private int spellNo;
    Image image;
    PlayerSpellsManager spells;
    public float cooldown;
    public Sprite[] magicSpells;
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        image = GetComponent<Image>();
        image.enabled = true;
        spells = FindObjectOfType<PlayerSpellsManager>();
        image.sprite = magicSpells[spells.currentSpell[0]];
        
        //spells.newSpellSelected += UpdateIcon;
    }

    private void Update()
    {
        if (cooldown > 0)
        {
            image.color = new Color32(255, 255, 255, 150);
        }
        else
        {
            image.color = new Color32(255, 255, 255, 255);
        }
    }

    public void UpdateIcon()
    {
        image.sprite = magicSpells[spells.currentSpell[0]];
    }
    public void Cooldown(float cooldown)
    {
        this.cooldown += cooldown;
    }

}
