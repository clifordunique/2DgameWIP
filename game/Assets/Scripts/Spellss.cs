using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spellss : MonoBehaviour
{
    public static Spellss Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    public int[] currentSpell = new int[2];
    public enum MagicSpells { FIRESTRIKE, THUNDERSTRIKE, XSTRIKE, YSTRIKE };
    void Start()
    {
        currentSpell[0] = (int)MagicSpells.FIRESTRIKE;
        currentSpell[1] = (int)MagicSpells.THUNDERSTRIKE;
    }

    void SelectNewSpell(int newSpell)
    {
        currentSpell[1] = currentSpell[0];
        currentSpell[0] = newSpell;
    }

    // Use this for initialization



}
