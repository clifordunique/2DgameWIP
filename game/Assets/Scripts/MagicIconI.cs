using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MagicIconI : MonoBehaviour
{
    public static MagicIconI Instance { get; private set; }
    private int spellNo;
    Image image;
    PlayerSpellsManager spells;
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
        image.sprite = magicSpells[spells.currentSpell[1]];
    }
    void UpdateIcon()
    {
        image.sprite = magicSpells[spells.currentSpell[1]];
    }


}
